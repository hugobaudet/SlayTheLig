using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum FightStep
{
    AnimationCard,
    PlayerChoice,
    PlayerComboChoice,
    PlayerAttack,
    EnemyTurn,
    EndFight,
}

[System.Serializable]
public class Card
{
    public Attack card;
    public int number;

    public Card(Attack attack, int value)
    {
        this.card = attack;
        this.number = value;
    }
}

public class FightSystem : MonoBehaviour
{
    public static FightSystem instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one PlayerBehaviour in the scene !");
            Destroy(gameObject);
            return;
        }
        instance = this;

        DOTween.SetTweensCapacity(500, 500);
        player.InitializeCharacter();
        enemy.InitializeCharacter();
        uiManager.UpdateUIArmour();
        uiManager.UpdateUIActionPoint();
        uiManager.UpdateUIHealthBar();
    }

    public PlayerBehaviour player;
    public EnemyBehaviour enemy;

    public CardManager cardManager;
    public UIManager uiManager;

    private FightStep currentFightStep;

    public List<Card> deck;

    private Card lastattack;

    public UnityEvent endTurnEvent;

    private void Start()
    {
        lastattack = null;
        cardManager.Initialize();
        StartPlayerTurn();
        uiManager.UpdateUIActionPoint();
        uiManager.UpdateUIArmour();
    }

    void StartPlayerTurn()
    {
        if (currentFightStep == FightStep.EndFight) return;
        currentFightStep = FightStep.AnimationCard;
        enemy.ChoseNextAttack();
        player.ReInitializeBeforeTurn();
        cardManager.ResetCardsInHand();
    }

    public void StartPlayerChoice()
    {
        if (currentFightStep != FightStep.AnimationCard) return;
        PlayNextPhase();
    }

    //Used by the button
    public void EndPlayerTurn()
    {
        if (currentFightStep != FightStep.PlayerChoice) return;
        endTurnEvent.Invoke();
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        currentFightStep = FightStep.EnemyTurn;
        enemy.ReInitializeBeforeTurn();
        enemy.PlayNextAttack();
    }

    public void AnimationCardDone(bool cardAnimation)
    {
        currentFightStep = cardAnimation ? FightStep.PlayerChoice : FightStep.AnimationCard;
    }

    public bool PlayACard(Attack attack, int index, bool comboPossible = true)
    {
        if (!player.CanPlayACard(attack) || currentFightStep != FightStep.PlayerChoice) return false;
        player.currentActionCost -= attack.actionCost;
        uiManager.UpdateUIActionPoint();
        currentFightStep = FightStep.PlayerAttack;
        switch (attack.attackType)
        {
            case AttackType.SimpleAttack:
                enemy.TakeDamage(attack.basicDamage);
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.ComboAttack:
                if (cardManager.IsComboPossible(attack) && comboPossible)
                {
                    if (lastattack != null)
                    {
                        enemy.TakeDamage(attack.comboDamage);
                        cardManager.RemoveComboPieces(attack);
                        cardManager.RemoveCardAt(index);
                        return true;
                    }
                    currentFightStep = FightStep.PlayerComboChoice;
                    lastattack = new Card(attack, index);
                    uiManager.DisplayUICombo();
                    return false;
                }
                switch (attack.noComboAttackType)
                {
                    case AttackType.SimpleAttack:
                        enemy.TakeDamage(attack.basicDamage);
                        break;
                    case AttackType.Heal:
                        player.HealCharacter(attack.basicHeal);
                        break;
                    case AttackType.Buff:
                        player.ApplyBuff(attack);
                        break;
                    case AttackType.Defense:
                        player.AddArmour(attack.basicDefense);
                        break;
                    default:
                        break;
                }
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.Heal:
                player.HealCharacter(attack.basicHeal);
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.Buff:
                player.ApplyBuff(attack);
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.Defense:
                player.AddArmour(attack.basicDefense);
                cardManager.RemoveCardAt(index);
                return true;
            default:
                return true;
        }
    }

    //IT WORKS
    public void PlayCombo(bool combo)
    {
        currentFightStep = FightStep.PlayerChoice;
        PlayACard(lastattack.card, lastattack.number, combo);
        lastattack = null;
    }
    
    //IT WORKS
    public void WinLose(bool win)
    {
        currentFightStep = FightStep.EndFight;
        uiManager.DisplayEndMenu(win);
        foreach (CardBehaviour item in cardManager.cardBehaviours)
        {
            item.enabled = false;
        }
    }

    public void PlayNextPhase()
    {
        switch (currentFightStep)
        {
            case FightStep.AnimationCard:
            case FightStep.PlayerAttack:
                currentFightStep = FightStep.PlayerChoice;
                return;
            case FightStep.EnemyTurn:
                StartPlayerTurn();
                return;
            default:
                return;
        }
    }
}

