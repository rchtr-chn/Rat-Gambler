using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandManagerScript : MonoBehaviour
{
    public GameObject cardPrefab; //assign in inspector
    public Transform handTransform; //root of hand pos
    [SerializeField] float fanSpread = -8f; //how much to spread cards in hand
    [SerializeField] float horizontalSpacing = 100f; //spacing between cards
    [SerializeField] float verticalSpacing = 30f; //spacing between rows if needed

    public List<GameObject> onHandCards = new List<GameObject>(); //list of cards in deck

    private void Start()
    {
        
    }

    private void Update()
    {
        if(onHandCards.Count > 0 && SceneManager.GetActiveScene().name == "GameplayScene")
        {
            CheckForLegalPlays();
        }
    }

    public void AddCardToHand(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        onHandCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().isCopied = false;

        //flip card if in enemy hand
        if (gameObject.name == "EnemyHandManager")
        {
            newCard.GetComponent<CardFlipScript>().FlipCardInstant();
        }

        UpdateHandPositions();
    }
    public void AddCopiedCardToHand(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        onHandCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().isCopied = true;

        //flip card if in enemy hand
        if (gameObject.name == "EnemyHandManager")
        {
            newCard.GetComponent<CardFlipScript>().FlipCardInstant();
        }

        UpdateHandPositions();
    }

    public void RemoveCardFromHand(Card cardData)
    {
        for (int i = 0; i < onHandCards.Count; i++)
        {
            if (onHandCards[i].GetComponent<CardDisplay>().cardData == cardData)
            {
                Destroy(onHandCards[i]);
                onHandCards.RemoveAt(i);
                break;
            }
        }

        UpdateHandPositions();
    }

    public void UpdateHandPositions()
    {
        int cardCount = onHandCards.Count;

        if (cardCount == 1)
        {
            onHandCards[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            onHandCards[0].transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        for(int i=0; i < cardCount; i++)
        {
            float rotAngle = (fanSpread * (i - (cardCount-1) / 2f));
            onHandCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotAngle);

            float xOffset = (horizontalSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPos = (2f * i / (cardCount - 1) - 1f); // Normalize position between -1 and 1
            float yOffset = verticalSpacing * (1 - normalizedPos * normalizedPos); // Adjust vertical position based on normalized position

            //set card pos
            onHandCards[i].transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
    }

    void CheckForLegalPlays()
    {
        FieldManagerScript fieldManager = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();

        for (int i = 0; i < onHandCards.Count; i++)
        {
            GameObject obj = onHandCards[i];
            Card card = obj.GetComponent<CardDisplay>().cardData;
            if (card.cardType.Contains(Card.CardType.Poker))
            {
                //int potentialValue = fieldManager.totalCardValue + card.cardPoints;

                //dim illegal cards in player hand
                if (gameObject.name == "PlayerHandManager")
                {
                    DimIllegalCards(obj, fieldManager.totalCardValue >= 21);
                }

                if (fieldManager.totalCardValue >= 21)
                {
                    obj.GetComponent<CardDisplay>().isPlayable = false;
                    obj.GetComponent<CardMovementScript>().enabled = false;
                }
                else
                {
                    obj.GetComponent<CardDisplay>().isPlayable = true;
                    obj.GetComponent<CardMovementScript>().enabled = true;
                }
            }
        }
    }

    public void ReturnAllCardsToDeck()
    {
        //check if deck belonged to player or enemy
        DeckManagerScript deckManager;
        if (gameObject.name == "PlayerHandManager")
        {
            deckManager = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        }
        else
        {
            deckManager = GameObject.FindGameObjectWithTag("EnemyDeckManager").GetComponent<DeckManagerScript>();
        }

        if(deckManager == null)
        {
            Debug.LogError("DeckManager not found!");
            return;
        }

        for (int i = onHandCards.Count - 1; i >= 0; i--)
        {
            GameObject obj = onHandCards[i];
            if (obj.GetComponent<CardDisplay>().isCopied)
            {
                onHandCards.RemoveAt(i);
                Destroy(obj);
            }
            else 
            {
                Card cardData = obj.GetComponent<CardDisplay>().cardData;
                deckManager.playingDeck.Add(cardData);
                onHandCards.RemoveAt(i);
                deckManager.currentHandSize--;
                Destroy(obj);
            }
        }
        UpdateHandPositions();
    }

    void DimIllegalCards(GameObject obj, bool isPlayable)
    {
        obj.GetComponent<CardDisplay>().highlightImage.gameObject.SetActive(isPlayable);
    }
}
