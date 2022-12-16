using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Attack> currentDeck, currentHand, currentDiscardPile;
 
    public List<CardBehaviour> cardBehaviours;

    public void Initialize()
    {
        currentDeck.Clear();
        currentHand.Clear();
        currentDiscardPile.Clear();
        foreach (Card item in FightSystem.instance.deck)
        {
            for (int i = 0; i < item.number; i++)
            {
                currentDeck.Add(item.card);
            }
        }
        for (int i = 0; i < 4; i++)
        {
            currentHand.Add(null);
        }
        ResetCardsInHand();
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].cardIndex = i;
        }
    }

    public bool IsComboPossible(Attack attackCombo)
    {
        List<Attack> copyHand = new List<Attack>(currentHand);
        foreach (Card item in attackCombo.comboPieces)
        {
            for (int i = 0; i < item.number; i++)
            {
                if (!copyHand.Contains(item.card))
                {
                    return false;
                }
                else
                {
                    copyHand.Remove(item.card);
                }
            }
        }
        return true;
    }

    public void RemoveCardAt(int index)
    {
        if (currentHand[index] == null) return;
        currentDiscardPile.Add(currentHand[index]);
        currentHand[index] = null;
        cardBehaviours[index].ChangeSide(true);
        FightSystem.instance.uiManager.UpdateUIPiles(currentDeck.Count, currentDiscardPile.Count);
    }

    public void RemoveComboPieces(Attack attack)
    {
        for (int i = 0; i < attack.comboPieces.Count; i++)
        {
            for (int j = 0; j < attack.comboPieces[i].number; j++)
            {
                for (int k = 0; k < cardBehaviours.Count; k++)
                {
                    if (attack.comboPieces[i].card == cardBehaviours[k].attack)
                    {
                        RemoveCardAt(k);
                        break;
                    }
                }
            }
        }
    }

    private void ReplaceCards()
    {

    }

    public void ResetCardsInHand()
    {
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].ChangeSide(true);
        }
        for (int i = 0; i < 4; i++)
        {
            if (currentHand[i] == null)
            {
                if (currentDeck.Count != 0)
                {
                    int index = Random.Range(0, currentDeck.Count);
                    currentHand[i] = currentDeck[index];
                    currentDeck.RemoveAt(index);
                }
                else
                {
                    int index = Random.Range(0, currentDiscardPile.Count);
                    currentHand[i] = currentDiscardPile[index];
                    currentDiscardPile.RemoveAt(index);
                }
            }
        }
        if (currentDeck.Count == 0)
        {
            currentDeck = new(currentDiscardPile);
            currentDiscardPile.Clear();
        }
        for (int i = 0; i < currentHand.Count; i++)
        {
            cardBehaviours[i].SetNewAttack(currentHand[i]);
        }
        FightSystem.instance.uiManager.UpdateUIPiles(currentDeck.Count, currentDiscardPile.Count);
    }
}
