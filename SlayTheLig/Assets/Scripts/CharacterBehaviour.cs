using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField] 
    protected Character character;

    [SerializeField]
    protected int maxHP;

    protected int currentHP;
    protected bool isAlive;

    protected virtual void Start()
    {
        isAlive = true;
        currentHP = maxHP;
    }

    protected virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(name + " a actuellement " + currentHP + " HPs!");
        if (currentHP <= 0)
        {
            isAlive = false;
            currentHP = 0;
        }
    }

    public virtual void DamageEnemy(CharacterBehaviour enemy, Attack attack)
    {
        enemy.TakeDamage(attack.basicDamage);
    }
}
