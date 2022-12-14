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
        currentDiscardPile.Add(currentHand[index]);
        currentHand[index] = null;
        FightSystem.instance.uiManager.UpdateUIPiles(currentDeck.Count, currentDiscardPile.Count);
    }

    public void RemoveComboPieces(Attack attack)
    {
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            for (int y = 0; y < attack.comboPieces.Count; y++)
            {
                if (attack.comboPieces[y].card == cardBehaviours[i].attack)
                {
                    RemoveCardAt(i);
                    cardBehaviours[i].ChangeAppearance(true);
                    break;
                }
            }
        }
    }

    public void ResetCardsInHand()
    {
        for (int i = 0; i < currentHand.Count; i++)
        {
            if (currentHand[i] != null)
            {
                currentDeck.Add(currentHand[i]);
                currentHand[i] = null;
            }
        }
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].ChangeAppearance(true);
        }
        for (int i = 0; i < 4; i++)
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
