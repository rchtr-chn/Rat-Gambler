using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandManagerScript : MonoBehaviour
{
    public GameObject CardPrefab; //assign in inspector
    public Transform HandTransform; //root of hand pos
    [SerializeField] private float _fanSpread = -8f; //how much to spread cards in hand
    [SerializeField] private float _horizontalSpacing = 100f; //spacing between cards
    [SerializeField] private float _verticalSpacing = 30f; //spacing between rows if needed

    public List<GameObject> OnHandCards = new List<GameObject>(); //list of cards in deck

    private void Update()
    {
        if(OnHandCards.Count > 0 && SceneManager.GetActiveScene().name == "GameplayScene")
        {
            CheckForLegalPlays();
        }
    }

    public void AddCardToHand(Card cardData)
    {
        //instantiate card
        GameObject newCard = Instantiate(CardPrefab, HandTransform.position, Quaternion.identity, HandTransform);
        OnHandCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().CardData = cardData;
        newCard.GetComponent<CardDisplay>().IsCopied = false;

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
        GameObject newCard = Instantiate(CardPrefab, HandTransform.position, Quaternion.identity, HandTransform);
        OnHandCards.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().CardData = cardData;
        newCard.GetComponent<CardDisplay>().IsCopied = true;

        //flip card if in enemy hand
        if (gameObject.name == "EnemyHandManager")
        {
            newCard.GetComponent<CardFlipScript>().FlipCardInstant();
        }

        UpdateHandPositions();
    }

    public void RemoveCardFromHand(Card cardData)
    {
        for (int i = 0; i < OnHandCards.Count; i++)
        {
            if (OnHandCards[i].GetComponent<CardDisplay>().CardData == cardData)
            {
                Destroy(OnHandCards[i]);
                OnHandCards.RemoveAt(i);
                break;
            }
        }

        UpdateHandPositions();
    }

    public void UpdateHandPositions()
    {
        int cardCount = OnHandCards.Count;

        if (cardCount == 1)
        {
            OnHandCards[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
            OnHandCards[0].transform.localPosition = new Vector3(0, 0, 0);
            return;
        }

        for(int i=0; i < cardCount; i++)
        {
            float rotAngle = (_fanSpread * (i - (cardCount-1) / 2f));
            OnHandCards[i].transform.localRotation = Quaternion.Euler(0, 0, rotAngle);

            float xOffset = (_horizontalSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPos = (2f * i / (cardCount - 1) - 1f); // Normalize position between -1 and 1
            float yOffset = _verticalSpacing * (1 - normalizedPos * normalizedPos); // Adjust vertical position based on normalized position

            //set card pos
            OnHandCards[i].transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
    }

    void CheckForLegalPlays()
    {
        FieldManagerScript fieldManager = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();

        for (int i = 0; i < OnHandCards.Count; i++)
        {
            GameObject obj = OnHandCards[i];
            Card card = obj.GetComponent<CardDisplay>().CardData;
            if (card.CardType.Contains(Card.CardTypes.Poker))
            {
                //int potentialValue = fieldManager.totalCardValue + card.cardPoints;

                //dim illegal cards in player hand
                if (gameObject.name == "PlayerHandManager")
                {
                    DimIllegalCards(obj, fieldManager.TotalCardValue >= 21);
                }

                if (fieldManager.TotalCardValue >= 21)
                {
                    obj.GetComponent<CardDisplay>().IsPlayable = false;
                    obj.GetComponent<CardMovementScript>().enabled = false;
                }
                else
                {
                    obj.GetComponent<CardDisplay>().IsPlayable = true;
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

        for (int i = OnHandCards.Count - 1; i >= 0; i--)
        {
            GameObject obj = OnHandCards[i];
            if (obj.GetComponent<CardDisplay>().IsCopied)
            {
                OnHandCards.RemoveAt(i);
                Destroy(obj);
            }
            else 
            {
                Card cardData = obj.GetComponent<CardDisplay>().CardData;
                deckManager.PlayingDeck.Add(cardData);
                OnHandCards.RemoveAt(i);
                deckManager.CurrentHandSize--;
                Destroy(obj);
            }
        }
        UpdateHandPositions();
    }

    void DimIllegalCards(GameObject obj, bool isPlayable)
    {
        obj.GetComponent<CardDisplay>().HighlightImage.gameObject.SetActive(isPlayable);
    }
}
