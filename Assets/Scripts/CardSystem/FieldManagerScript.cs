using System.Collections.Generic;
using UnityEngine;

public class FieldManagerScript : MonoBehaviour
{
    public GameObject CardPrefab; //assign in inspector
    public Transform HandTransform; //root of hand pos
    [SerializeField] private float _fanSpread = 0f; //how much to spread cards in hand
    [SerializeField] private float _horizontalSpacing = 200f; //spacing between cards
    [SerializeField] private float _verticalSpacing = 30f; //spacing between rows if needed
    public int TotalCardValue = 0; //total value of cards in hand

    public List<GameObject> FieldCards = new List<GameObject>(); //list of cards in deck

    public void AddCardToField (Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(CardPrefab, HandTransform.position, Quaternion.identity, HandTransform);
        newCard.GetComponent<CardMovementScript>().enabled = false; //disable movement script
        FieldCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().CardData = cardData;
        UpdateTotalCardValue(cardData);

        UpdateHandPositions();
    }
    public void AddCopiedCardToField(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(CardPrefab, HandTransform.position, Quaternion.identity, HandTransform);
        newCard.GetComponent<CardMovementScript>().enabled = false; //disable movement script
        FieldCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().CardData = cardData;
        newCard.GetComponent<CardDisplay>().IsCopied = true;
        UpdateTotalCardValue(cardData);

        UpdateHandPositions();
    }

    void UpdateHandPositions()
    {
        int cardCount = FieldCards.Count;

        if (cardCount == 1)
        {
            FieldCards[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            FieldCards[0].transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotAngle = (_fanSpread * (i - (cardCount - 1) / 2f));
            FieldCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotAngle);

            float xOffset = (_horizontalSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPos = (2f * i / (cardCount - 1) - 1f); // Normalize position between -1 and 1
            float yOffset = _verticalSpacing * (1 - normalizedPos * normalizedPos); // Adjust vertical position based on normalized position

            //set card pos
            FieldCards[i].transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
    }

    void UpdateTotalCardValue(Card cardData)
    {
        TotalCardValue = CalculateHandValue(FieldCards);
    }
    int CalculateHandValue(List<GameObject> hand)
    {
        int total = 0;
        int aceCount = 0;

        foreach (var card in hand)
        {
            if(card != null)
            {
                if (card.GetComponent<CardDisplay>().CardData.CardType.Contains(Card.CardTypes.Aces))
                {
                    aceCount++;
                    total += 1; // Count Ace as 1 first
                }
                else
                {
                    total += card.GetComponent<CardDisplay>().CardData.CardPoints;
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

        for (int i = FieldCards.Count - 1; i >= 0; i--)
        {
            GameObject obj = FieldCards[i];
            if (obj.GetComponent<CardDisplay>().IsCopied)
            {
                FieldCards.RemoveAt(i);
                Destroy(obj);
            }
            else
            {
                Card cardData = obj.GetComponent<CardDisplay>().CardData;
                deckManager.PlayingDeck.Add(cardData);
                FieldCards.RemoveAt(i);
                deckManager.CurrentHandSize--;
                Destroy(obj);
            }
        }
        UpdateHandPositions();
    }
}
