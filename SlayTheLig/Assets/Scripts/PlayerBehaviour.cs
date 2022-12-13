using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerBehaviour : CharacterBehaviour
{
    public static PlayerBehaviour instance;
    public int currentActionCost;

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

    public bool CheckPossibilityAttack(CharacterBehaviour enemy, Attack attack)
    {
        if (!isAlive) return false;
        //if (currentActionCost <= attack.actionCost) return false;

        DamageEnemy(enemy, attack);
        return true;
    }
}
