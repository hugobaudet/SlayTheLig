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

    private FightStep currentFightStep;

    public List<Card> deck;

    [SerializeField]
    private List<Stage> stages;

    private void Start()
    {
        currentFightStep = FightStep.AnimationEntry;
        cardManager.Initialize();
    }

    public void EndTurn()
    {

    }

    public bool PlayACard(Attack attack)
    {
        if (!player.CanPlayACard(attack)) return false;
        currentFightStep = FightStep.CardEffect;
        switch (attack.attackType)
        {
            case AttackType.SimpleAttack:
                enemy.TakeDamage(attack.basicDamage);
                return true;
            case AttackType.ComboAttack:
                if (true)
                {
                    enemy.TakeDamage(attack.comboDamage);
                }
                return true;
            case AttackType.Heal:
                return true;
            case AttackType.Buff:
                return true;
            case AttackType.Defense:
                return true;
            default:
                return true;
        }
    }
}

