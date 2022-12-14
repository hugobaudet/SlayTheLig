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
public class Stage
{
    public string name;

    public List<EnemyBehaviour> enemyList;
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

    [SerializeField]
    private List<Stage> stages;

    private void Start()
    {
        currentFightStep = FightStep.AnimationEntry;
        currentFightStep = FightStep.PlayerChoice;
        cardManager.Initialize();
        player.StartRound();
        uiManager.UpdateUIActionPoint();
    }

    public void EndTurn()
    {
        if (currentFightStep != FightStep.PlayerChoice) return;
        currentFightStep = FightStep.EnemyAttack;
        player.isTurnBuffed = false;
        StartTurn();
    }

    void StartTurn()
    {
        currentFightStep = FightStep.PlayerChoice;
        cardManager.ResetCardsInHand();
        player.StartRound();
        uiManager.UpdateUIActionPoint();
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
                    enemy.TakeDamage(attack.basicDamage);
                    cardManager.RemoveCardAt(index);
                }
                return true;
            case AttackType.Heal:
                player.HealPlayer(attack.basicHeal);
                cardManager.RemoveCardAt(index);
                return true;
            case AttackType.Buff:
                player.isTurnBuffed = true;
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
}

