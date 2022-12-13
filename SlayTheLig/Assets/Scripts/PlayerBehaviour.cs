using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : CharacterBehaviour
{
    public int maxActionCost;

    private int currentActionCost;

    public bool isTurnBuffed;

    void StartMatch()
    {
        currentHP = maxHP;
        currentActionCost = maxActionCost;
    }

    public bool CanPlayACard(Attack attack)
    {
        return currentActionCost >= attack.actionCost;
    }
}
