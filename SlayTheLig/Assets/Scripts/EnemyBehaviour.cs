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

    protected override void Awake()
    {
        base.Awake();
        currentPhase = 1;
        FightSystem.instance.uiManager.ChangePhaseColor(phaseColors.Evaluate(1-(currentPhase/3f)));
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
                FightSystem.instance.WinLose(false);
                return;
            }
            FightSystem.instance.uiManager.ChangePhaseColor(phaseColors.Evaluate(1- (currentPhase / 3f)));
            currentHP = maxHP;
        }
    }

    public void ChoseNextAttack()
    {
        List<EnemyAttack> possibleAttacks = enemyAttacks.FindAll(x => x.phase == currentPhase);
        if (currentHP == maxHP)
        {
            possibleAttacks.Remove(enemyAttacks.Find(x => x.type == EnemyAttacksType.Heal));
        }
        nextAttack = possibleAttacks[Random.Range(0, possibleAttacks.Count)];
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
