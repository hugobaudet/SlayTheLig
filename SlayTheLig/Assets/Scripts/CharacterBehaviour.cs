using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    protected int maxHP;

    protected int currentHP;
    protected int armourAmount;
    protected bool isAlive;

    protected virtual void Start()
    {
        isAlive = true;
        currentHP = maxHP;
        armourAmount = 0;
    }

    public virtual void TakeDamage(int damage)
    {
        damage *= FightSystem.instance.player.isTurnBuffed ? 2 : 1;
        damage -= armourAmount;
        currentHP -= Mathf.Clamp(damage, 0, currentHP);
        if (currentHP <= 0)
        {
            isAlive = false;
            currentHP = 0;
        }
    }

    public void AddArmour(int armourAmount)
    {
        this.armourAmount += armourAmount;
    }
}
