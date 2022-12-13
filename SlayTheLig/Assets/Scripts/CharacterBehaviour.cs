using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    protected int maxHP;

    protected int currentHP;
    protected bool isAlive;

    protected virtual void Start()
    {
        isAlive = true;
        currentHP = maxHP;
    }

    public virtual void TakeDamage(int damage)
    {
        if (FightSystem.instance.player.isTurnBuffed)
        {
            damage *= 2;
        }
        currentHP -= Mathf.Clamp(damage, 0, currentHP);
        Debug.Log(name + " a actuellement " + currentHP + " HPs!");
        if (currentHP <= 0)
        {
            isAlive = false;
            currentHP = 0;
        }
    }
}
