using DG.Tweening;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    public int maxHP, knockBackForce;

    [HideInInspector]
    public int currentHP, armourAmount;
    protected bool isAlive;
    protected Vector3 initialPosition;
    protected Image sprite;

    protected Tween knockBackTween;

    public Gradient phaseColors;

    public virtual void InitializeCharacter()
    {
        isAlive = true;
        currentHP = maxHP;
        armourAmount = 0;
        initialPosition = transform.position;
        sprite = GetComponent<Image>();
    }

    public virtual void ReInitializeBeforeTurn()
    {
        armourAmount = 0;
        FightSystem.instance.uiManager.UpdateUIArmour();
    }

    public virtual void TakeDamage(int damage, int direction = -1)
    {
        initialPosition = transform.position;
        int tmpDamage = damage;
        damage -= armourAmount;
        armourAmount -= Mathf.Clamp(tmpDamage, 0, armourAmount);
        currentHP -= Mathf.Clamp(damage, 0, currentHP);
        StartCoroutine(KnockBack(direction));
        CheckDeath();
        FightSystem.instance.uiManager.UpdateUIArmour();
        FightSystem.instance.uiManager.UpdateUIHealthBar();
    }

    IEnumerator KnockBack(int direction)
    {
        knockBackTween = transform.DOMove(initialPosition + Vector3.right * direction * knockBackForce,.2f).SetEase(Ease.OutExpo);
        sprite.color = Color.red;
        yield return knockBackTween.WaitForCompletion();
        knockBackTween = transform.DOMove(initialPosition,.3f);
        sprite.color = Color.white;
        yield return knockBackTween.WaitForCompletion();
        FightSystem.instance.PlayNextPhase();
    }

    protected virtual void CheckDeath()
    {
        if (currentHP <= 0)
        {
            isAlive = false;
            currentHP = 0;
            FightSystem.instance.WinLose(false);
            sprite.DOFade(0, 1);
            knockBackTween.Kill();
        }
    }

    public virtual void AddArmour(int armourAmount)
    {
        this.armourAmount += armourAmount;
        FightSystem.instance.uiManager.UpdateUIArmour();
        FightSystem.instance.PlayNextPhase();
    }

    public virtual void HealCharacter(int healAmount)
    {
        currentHP += Mathf.Clamp(healAmount, 0, maxHP - currentHP);
        FightSystem.instance.uiManager.UpdateUIHealthBar();
        FightSystem.instance.PlayNextPhase();
    }
}
