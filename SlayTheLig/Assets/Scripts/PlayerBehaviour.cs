using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : CharacterBehaviour
{
    public int maxActionCost;

    [HideInInspector]
    public int currentActionCost, isTurnBuffed, isDamageBuffed, isArmourBuffed, isHealBuffed;

    public override void ReInitializeBeforeTurn()
    {
        base.ReInitializeBeforeTurn();
        currentActionCost = maxActionCost;
        FightSystem.instance.uiManager.UpdateUIActionPoint();
        isTurnBuffed = 1;
        isHealBuffed = 1;
        isDamageBuffed = 1;
        isArmourBuffed = 1;
    }

    public bool CanPlayACard(Attack attack)
    {
        return currentActionCost >= attack.actionCost;
    }

    public void ApplyBuff(Attack attack)
    {
        switch (attack.noComboBuffAttackType)
        {
            case AttackType.SimpleAttack:
            case AttackType.ComboAttack:
                isDamageBuffed *= attack.buffPower;
                break;
            case AttackType.Heal:
                isHealBuffed *= attack.buffPower;
                break;
            case AttackType.Buff:
                isTurnBuffed *= attack.buffPower;
                break;
            case AttackType.Defense:
                isArmourBuffed *= attack.buffPower;
                break;
            default:
                break;
        }
    }

    public override void HealCharacter(int healAmount)
    {
        healAmount *= isTurnBuffed * isHealBuffed;
        base.HealCharacter(healAmount);
    }

    public override void AddArmour(int armourAmount)
    {
        armourAmount *= isTurnBuffed * isArmourBuffed;
        base.AddArmour(armourAmount);
    }
}
