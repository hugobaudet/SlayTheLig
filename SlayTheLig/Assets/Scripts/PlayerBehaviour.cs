using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : CharacterBehaviour
{
    public int maxActionCost;

    [HideInInspector]
    public int currentActionCost;

    [HideInInspector]
    public int isTurnBuffed;

    public override void StartRound()
    {
        base.StartRound();
        currentActionCost = maxActionCost;
        isTurnBuffed = 1;
    }

    public bool CanPlayACard(Attack attack)
    {
        return currentActionCost >= attack.actionCost;
    }

    public void ApplyBuff(Attack attack)
    {
        isTurnBuffed *= attack.buffPower;
    }

    public override void HealCharacter(int healAmount)
    {
        healAmount *= isTurnBuffed;
        base.HealCharacter(healAmount);
    }
}
