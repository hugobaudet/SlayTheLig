using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public enum FightStep
{
    AnimationEntry,
    AnimationCard,
    PlayerChoice,
    PlayerAttack,
    CardEffect,
    EnemyAttack,
    EndFight,
}

[System.Serializable]
public class Card
{
    public Attack card;
    public int number;
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
    }

    public PlayerBehaviour player;
    public EnemyBehaviour enemy;

    public CardManager cardManager;
    public UIManager uiManager;

    private FightStep currentFightStep;

    public List<Card> deck;

    private void Start()
    {
        cardManager.Initialize();
        StartTurn();
        enemy.StartRound();
        uiManager.UpdateUIActionPoint();
        uiManager.UpdateUIArmour();
    }

    void StartTurn()
    {
        currentFightStep = FightStep.PlayerChoice;
        cardManager.ResetCardsInHand();
        player.StartRound();
        enemy.ChoseNextAttack();
        uiManager.UpdateUIActionPoint();

    }

    public void EndTurn()
    {
        if (currentFightStep != FightStep.PlayerChoice) return;
        StartEnemyTurn();
    }

    void StartEnemyTurn()
    {
        enemy.StartRound();
        currentFightStep = FightStep.EnemyAttack;
        enemy.PlayNextAttack();
        StartTurn();
    }

    public bool PlayACard(Attack attack, int index)
    {
        if (!player.CanPlayACard(attack) || currentFightStep != FightStep.PlayerChoice) return false;
        //currentFightStep = FightStep.CardEffect;
        player.currentActionCost -= attack.actionCost;
        uiManager.UpdateUIActionPoint();
        switch (attack.attackType)
        {
            case AttackType.SimpleAttack:
                enemy.TakeDamage(attack.basicDamage);
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.ComboAttack:
                if (cardManager.IsComboPossible(attack))
                {
                    Debug.Log("COMBO");
                    enemy.TakeDamage(attack.comboDamage);
                    cardManager.RemoveCardAt(index);
                    cardManager.RemoveComboPieces(attack);
                }
                else
                {
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
                }
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

    public void WinLose(bool win)
    {
        currentFightStep = FightStep.EndFight;
        if (win)
        {
            Debug.Log("Partie gagnée");
        }
        else
        {
            Debug.Log("Partie perdue");
        }
    }
}

