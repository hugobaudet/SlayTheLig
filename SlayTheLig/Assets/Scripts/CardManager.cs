using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public List<Attack> currentDeck, currentHand, currentDiscardPile;
 
    public List<CardBehaviour> cardBehaviours;

    private float cardWidth;
    private Transform cardPlacement;

    public void Initialize()
    {
        cardWidth = ((RectTransform)cardBehaviours[0].transform).sizeDelta.x;
        foreach (CardBehaviour item in cardBehaviours)
        {
            item.Initialize();
        }
        cardPlacement = FightSystem.instance.uiManager.cardPlacement;
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
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].cardIndex = i;
        }
    }

    //IT WORKS
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
        ReplaceCards();
    }

    //IT WORKS
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
    
    //IT MIGHT WORK
    private void ReplaceCards()
    {
        List<CardBehaviour> cards = cardBehaviours.FindAll(x => x.attack != null);
        if (cards.Count == 0) return;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetInitialPosition(cardPlacement.position.x + (cardWidth * i) - ((cardWidth/2f) * (cards.Count -1)));
        }
    }

    public void ResetCardsInHand()
    {
        for (int i = 0; i < currentHand.Count; i++)
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
        ReplaceCards();
    }
}
