using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManagerScript : MonoBehaviour
{
    public GameObject cardPrefab; //assign in inspector
    public Transform handTransform; //root of hand pos
    [SerializeField] float fanSpread = 0f; //how much to spread cards in hand
    [SerializeField] float horizontalSpacing = 200f; //spacing between cards
    [SerializeField] float verticalSpacing = 30f; //spacing between rows if needed
    public int totalCardValue = 0; //total value of cards in hand

    public List<GameObject> fieldCards = new List<GameObject>(); //list of cards in deck

    public void AddCardToField (Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        newCard.GetComponent<CardMovementScript>().enabled = false; //disable movement script
        fieldCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        UpdateTotalCardValue(cardData);

        UpdateHandPositions();
    }
    public void AddCopiedCardToField(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        newCard.GetComponent<CardMovementScript>().enabled = false; //disable movement script
        fieldCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().isCopied = true;
        UpdateTotalCardValue(cardData);

        UpdateHandPositions();
    }

    void UpdateHandPositions()
    {
        int cardCount = fieldCards.Count;

        if (cardCount == 1)
        {
            fieldCards[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            fieldCards[0].transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            fieldCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotAngle);

            float xOffset = (horizontalSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPos = (2f * i / (cardCount - 1) - 1f); // Normalize position between -1 and 1
            float yOffset = verticalSpacing * (1 - normalizedPos * normalizedPos); // Adjust vertical position based on normalized position

            //set card pos
            fieldCards[i].transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
    }

    void UpdateTotalCardValue(Card cardData)
    {
        totalCardValue = CalculateHandValue(fieldCards);
    }
    int CalculateHandValue(List<GameObject> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            if(card != null)
            {
                if (card.GetComponent<CardDisplay>().cardData.cardType.Contains(Card.CardType.Aces))
                {
                    aceCount++;
                    total += 1; // Count Ace as 1 first
                }
                else
                {
                    total += card.GetComponent<CardDisplay>().cardData.cardPoints;
                }
            }
        }

        // Upgrade some Aces from 1 to 11 if it won’t bust
        while (aceCount > 0 && total + 10 <= 21)
        {
            total += 10;
            aceCount--;
        }

        return total;
    }

    public void ReturnAllCardsToDeck()
    {
        //check if deck belonged to player or enemy
        DeckManagerScript deckManager;
        if (gameObject.name == "PlayerFieldManager")
        {
            deckManager = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        }
        else
        {
            deckManager = GameObject.FindGameObjectWithTag("EnemyDeckManager").GetComponent<DeckManagerScript>();
        }

        for (int i = fieldCards.Count - 1; i >= 0; i--)
        {
            GameObject obj = fieldCards[i];
            if (obj.GetComponent<CardDisplay>().isCopied)
            {
                fieldCards.RemoveAt(i);
                Destroy(obj);
            }
            else
            {
                Card cardData = obj.GetComponent<CardDisplay>().cardData;
                deckManager.playingDeck.Add(cardData);
                fieldCards.RemoveAt(i);
                deckManager.currentHandSize--;
                Destroy(obj);
            }
        }
        UpdateHandPositions();
    }
}
