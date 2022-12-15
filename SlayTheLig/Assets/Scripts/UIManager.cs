using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerBehaviour player;

    [SerializeField]
    private EnemyBehaviour enemy;

    [SerializeField]
    private Image playerHealthBar, enemyHealthBar;

    [SerializeField]
    private TMP_Text actionPoint, nbCardInDiscardPile, nbCardInDeck, nbArmourPlayer, nbAmourEnemy;

    [SerializeField]
    private GameObject uiCombo;

    public void UpdateUIActionPoint()
    {
        actionPoint.text = player.currentActionCost + " / " + player.maxActionCost;
    }

    public void UpdateUIPiles(int deck, int discard)
    {
        nbCardInDeck.text = deck.ToString();
        nbCardInDiscardPile.text = discard.ToString();
    }

    public void UpdateUIArmour()
    {
        nbArmourPlayer.text = player.armourAmount.ToString();
        nbAmourEnemy.text = enemy.armourAmount.ToString();
    }

    public void UpdateUIHealthBar()
    {
        playerHealthBar.fillAmount = player.currentHP / (float)player.maxHP;
        playerHealthBar.color = player.phaseColors.Evaluate(player.currentHP / (float)player.maxHP);
        enemyHealthBar.fillAmount = enemy.currentHP / (float)enemy.maxHP;
    }

    public void ChangePhaseColor(Color color)
    {
        enemyHealthBar.color = color;
    }

    public void DisplayUICombo()
    {
        uiCombo.SetActive(true);
    }

    public void UseCombo(bool use)
    {
        FightSystem.instance.PlayCombo(use);
        uiCombo.SetActive(false);
    }
}
