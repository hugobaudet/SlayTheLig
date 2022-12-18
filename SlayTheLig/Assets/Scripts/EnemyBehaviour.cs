using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

[System.Serializable]
public class Phase
{
    [Range(1, 3)]
    public int phase;
    public Sprite sprite;
}

public class EnemyBehaviour : CharacterBehaviour
{
    public List<EnemyAttack> enemyAttacks;

    int currentPhase;

    public EnemyAttack nextAttack;
    private Image image;

    public List<Phase> phaseSprites;

    public override void InitializeCharacter()
    {
        base.InitializeCharacter();
        currentPhase = 1;
        image = GetComponent<Image>();
        image.sprite = phaseSprites.Find(x => x.phase == currentPhase).sprite;
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
            StartCoroutine(ChangeSprite());
        }
    }

    IEnumerator ChangeSprite()
    {
        yield return new WaitForSeconds(.5f);
        image.sprite = phaseSprites.Find(x => x.phase == currentPhase).sprite;
    }

    public void ChoseNextAttack()
    {
        List<EnemyAttack> possibleAttacks = enemyAttacks.FindAll(x => x.phase == currentPhase);
        if (currentHP == maxHP)
        {
            foreach (EnemyAttack item in enemyAttacks.FindAll(x => x.type == EnemyAttacksType.Heal))
            {
                possibleAttacks.Remove(item);
            }
        }
        nextAttack = possibleAttacks[Random.Range(0, possibleAttacks.Count)];
    }

    public override void TakeDamage(int damage, int direction = -1)
    {
        FightSystem.instance.player.LaunchAttackAnimation();
        damage *= FightSystem.instance.player.isTurnBuffed * FightSystem.instance.player.isDamageBuffed;
        base.TakeDamage(damage, 1);
    }

    public void PlayNextAttack()
    {
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
