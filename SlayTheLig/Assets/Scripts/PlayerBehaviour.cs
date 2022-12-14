using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : CharacterBehaviour
{
    public int maxActionCost;

    [HideInInspector]
    public int currentActionCost;

    public bool isTurnBuffed;

    public void StartRound()
    {
        currentActionCost = maxActionCost;
    }

    public bool CanPlayACard(Attack attack)
    {
        return currentActionCost >= attack.actionCost;
    }

    public void HealPlayer(int healAmount)
    {
        healAmount *= isTurnBuffed? 2 : 1;
        currentHP += Mathf.Clamp(healAmount, 0, maxHP - currentHP);
    }
}
