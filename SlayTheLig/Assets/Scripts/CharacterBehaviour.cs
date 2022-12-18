using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    public int maxHP;

    [HideInInspector]
    public int currentHP, armourAmount;
    protected bool isAlive;

    public Gradient phaseColors;

    protected virtual void Awake()
    {
        isAlive = true;
        currentHP = maxHP;
        armourAmount = 0;
    }

    public virtual void StartRound()
    {
        armourAmount = 0;
        FightSystem.instance.uiManager.UpdateUIArmour();
    }

    public virtual void TakeDamage(int damage)
    {
        damage -= armourAmount;
        if (damage > 0)
        {
            armourAmount = 0;
        }
        else
        {
            armourAmount -= damage;
        }
        FightSystem.instance.uiManager.UpdateUIArmour();
        currentHP -= Mathf.Clamp(damage, 0, currentHP);
        Debug.Log(name + " a pris " + Mathf.Clamp(damage, 0, currentHP) + " damages, il lui reste " + currentHP + "HPs.");
        CheckDeath();
        FightSystem.instance.uiManager.UpdateUIHealthBar();
    }

    protected virtual void CheckDeath()
    {
        if (currentHP <= 0)
        {
            Debug.Log(name + " est mort!");
            isAlive = false;
            currentHP = 0;
            FightSystem.instance.WinLose(false);
        }
    }

    public virtual void AddArmour(int armourAmount)
    {
        this.armourAmount += armourAmount;
        Debug.Log(name + " a gagné " + armourAmount + " d'armures, il possède " + this.armourAmount + " armures.");
        FightSystem.instance.uiManager.UpdateUIArmour();
    }

    public virtual void HealCharacter(int healAmount)
    {
        currentHP += Mathf.Clamp(healAmount, 0, maxHP - currentHP);
        Debug.Log(name + " a récupéré " + healAmount + " HPs, il possède désormais " + currentHP + "HPs.");
        FightSystem.instance.uiManager.UpdateUIHealthBar();
    }
}
