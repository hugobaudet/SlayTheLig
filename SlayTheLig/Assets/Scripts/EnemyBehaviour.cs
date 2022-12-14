using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyAttacksType
{
    SimpleAttack,
    Heal,
    Defense,
}

[System.Serializable]
public class EnemyAttack
{
    [Range(1, 3)]
    public int phase;
    public EnemyAttacksType type;
    public int value;
}

public class EnemyBehaviour : CharacterBehaviour
{
    public List<EnemyAttack> enemyAttacks;

    int currentPhase;
    public EnemyAttack nextAttack;

    protected override void Start()
    {
        base.Start();
        currentPhase = 1;
    }

    protected override void CheckDeath()
    {
        if (currentHP <= 0)
        {
            currentPhase++;
            if (currentPhase == 4)
            {
                isAlive = false;
                currentHP = 0;
                return;
            }
            currentHP = maxHP;
        }
    }

    public void ChoseNextAttack()
    {
        if (currentHP == maxHP)
        {
            nextAttack = enemyAttacks
                .FindAll(x => x.phase == currentPhase
                && x.type != EnemyAttacksType.Heal)[Random.Range(0, 2)];
        }
        else
        {
            nextAttack = enemyAttacks
                .FindAll(x => x.phase == currentPhase)[Random.Range(0, 3)];
        }
    }

    public override void TakeDamage(int damage)
    {
        damage *= FightSystem.instance.player.isTurnBuffed;
        base.TakeDamage(damage);
    }

    public void PlayNextAttack()
    {
        Debug.Log("Valeur de " + nextAttack.type + " = " + nextAttack.value);
        switch (nextAttack.type)
        {
            case EnemyAttacksType.SimpleAttack:
                FightSystem.instance.player.TakeDamage(nextAttack.value);
                break;
            case EnemyAttacksType.Heal:
                HealCharacter(nextAttack.value);
                break;
            case EnemyAttacksType.Defense:
                AddArmour(nextAttack.value);
                break;
            default:
                break;
        }
    }
}
