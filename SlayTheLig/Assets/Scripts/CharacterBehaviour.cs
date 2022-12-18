using DG.Tweening;
using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour
{
    [SerializeField]
    private Image shieldImage;
    private Tween shieldFadeTween, shieldScaleTween;

    public int maxHP, knockBackForce;

    [HideInInspector]
    public int currentHP, armourAmount;
    protected bool isAlive;
    protected Vector3 initialPosition;
    protected Image sprite;

    protected Tween knockBackTween;

    public Gradient phaseColors;

    public UnityEvent damageEvent, healEvent, shieldEvent;

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
        damageEvent.Invoke();
        StartCoroutine(KnockBack(direction));
        if (direction < 0)
        {
            StartCoroutine(FightSystem.instance.enemy.KnockBack(direction, false));
        }
        else
        {
            StartCoroutine(FightSystem.instance.player.KnockBack(direction, false));
        }
        CheckDeath();
        FightSystem.instance.uiManager.UpdateUIArmour();
        FightSystem.instance.uiManager.UpdateUIHealthBar();
    }

    public IEnumerator KnockBack(int direction, bool initial = true)
    {
        knockBackTween = transform.DOMove(initialPosition + Vector3.right * direction * knockBackForce,.2f).SetEase(Ease.OutExpo);
        if (initial)
        {
            sprite.color = Color.red;
        }
        yield return knockBackTween.WaitForCompletion();
        knockBackTween = transform.DOMove(initialPosition,.3f);
        if (initial)
        {
            sprite.color = Color.white;
        }
        yield return knockBackTween.WaitForCompletion();
        if (initial)
        {
            FightSystem.instance.PlayNextPhase();
        }
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
        shieldEvent.Invoke();
        FightSystem.instance.uiManager.UpdateUIArmour();
        StartCoroutine(ArmourAdded());
    }

    IEnumerator ArmourAdded()
    {
        shieldFadeTween = shieldImage.DOFade(1, .5f).SetEase(Ease.OutQuint);
        shieldScaleTween = shieldImage.transform.DOScale(2, .5f).SetEase(Ease.OutQuint);
        yield return shieldFadeTween.WaitForCompletion();
        shieldFadeTween = shieldImage.DOFade(0, 1f);
        shieldScaleTween = shieldImage.transform.DOScale(1, 1f);
        yield return shieldFadeTween.WaitForCompletion();
        FightSystem.instance.PlayNextPhase();
    }

    public virtual void HealCharacter(int healAmount)
    {
        currentHP += Mathf.Clamp(healAmount, 0, maxHP - currentHP);
        healEvent.Invoke();
        FightSystem.instance.uiManager.UpdateUIHealthBar();
        FightSystem.instance.PlayNextPhase();
    }
}
