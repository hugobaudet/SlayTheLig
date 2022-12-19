using DG.Tweening;
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
    private TMP_Text actionPoint, nbCardInDiscardPile, nbCardInDeck, nbArmourPlayer, nbAmourEnemy, nbHPPlayer, nbHPEnemy;

    [SerializeField]
    private GameObject uiCombo, uiVictoryMenu, uiDefeatMenu;

    public Transform minimuHeight, cardPlacement;

    public void UpdateUIActionPoint()
    {
        actionPoint.text = player.currentActionCost.ToString();
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
        nbHPPlayer.text = player.currentHP + "/" + player.maxHP;
        playerHealthBar.color = player.phaseColors.Evaluate(player.currentHP / (float)player.maxHP);
        enemyHealthBar.fillAmount = enemy.currentHP / (float)enemy.maxHP;
        nbHPEnemy.text = enemy.currentHP + "/" + enemy.maxHP;
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
        if (FightSystem.instance.currentFightStep != FightStep.PlayerComboChoice) return;
        FightSystem.instance.PlayCombo(use);
        uiCombo.SetActive(false);
    }

    public void DisplayEndMenu(bool victory)
    { 
        if (victory)
        {
            uiVictoryMenu.SetActive(victory);
            uiVictoryMenu.GetComponent<Image>().DOFade(1, 1);
            foreach (Transform item in uiVictoryMenu.transform)
            {
                item.GetComponent<Image>().DOFade(1, 1);
            }
        }
        else
        {
            uiDefeatMenu.SetActive(!victory);
            uiDefeatMenu.GetComponent<Image>().DOFade(1, 1);
            foreach (Transform item in uiDefeatMenu.transform)
            {
                item.GetComponent<Image>().DOFade(1, 1);
            }
        }
    }
}
