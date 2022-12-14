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

    public virtual void StartRound()
    {
        armourAmount = 0;
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armourAmount;
        currentHP -= Mathf.Clamp(damage, 0, currentHP);
        Debug.Log(name + " a pris " + Mathf.Clamp(damage, 0, currentHP) + " damages, il lui reste " + currentHP + "HPs.");
        CheckDeath();
    }

    protected virtual void CheckDeath()
    {
        if (currentHP <= 0)
        {
            Debug.Log(name + " est mort!");
            isAlive = false;
            currentHP = 0;
        }
    }

    public void AddArmour(int armourAmount)
    {
        this.armourAmount += armourAmount;
        Debug.Log(name + " a gagné " + armourAmount + " d'armures, il possède " + this.armourAmount + " armures.");
    }

    public virtual void HealCharacter(int healAmount)
    {
        currentHP += Mathf.Clamp(healAmount, 0, maxHP - currentHP);
        Debug.Log(name + " a récupéré " + healAmount + " HPs, il possède désormais " + currentHP + "HPs.");

    }
}
