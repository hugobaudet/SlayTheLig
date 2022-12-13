using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        ResetCardsInHand();
    }

    public void ResetCardsInHand()
    {
        for (int i = 0; i < currentHand.Count; i++)
        {
            currentDeck.Add(currentHand[i]);
            currentHand.RemoveAt(i);
        }
        currentHand.Clear();
        for (int i = 0; i < cardBehaviours.Count; i++)
        {
            cardBehaviours[i].ChangeAppearance(true);
        }
        for (int i = 0; i < 4; i++)
        {
            if (currentDeck.Count == 0)
            {
                currentDeck = new(currentDiscardPile);
                currentDiscardPile.Clear();
            }
            int index = Random.Range(0, currentDeck.Count);
            currentHand.Add(currentDeck[index]);
            currentDeck.RemoveAt(index);
        }
    }
}
