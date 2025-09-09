using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public DeckManagerScript deckManagerScript;
    public HandManagerScript handManagerScript;
    public Text[] placeHolderCardText;
    List<Card> shopCards = new List<Card>();
    public GameObject shopCardPrefab;

    public AudioManagerScript audioManager;

    public List<GameObject> cardPlaceHolders = new List<GameObject>();

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();
        audioManager.musicSource.clip = audioManager.levelSelectBGM;
        audioManager.musicSource.loop = true;
        audioManager.musicSource.Play();

        deckManagerScript = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        handManagerScript = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();

        //set game manager to player turn so cards can be dragged
        GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
        gameManager.IsPlayerTurn = true;

        ShowPlayerDeck();

        GetRandomCards();
    }

    void ShowPlayerDeck()
    {
        for(int i = 0; i < handManagerScript.onHandCards.Count - 1; i++)
        {
            handManagerScript.onHandCards[i].GetComponent<CardMovementScript>().cardPlay = new Vector2(0, 1200);
        }

        deckManagerScript.DrawEntireDeck();
    }

    void GetRandomCards()
    {
        Card[] cards = Resources.LoadAll<Card>("CardData/ShopCards");
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, cards.Length - 1);
            shopCards.Add(cards[randomIndex]);
        }

        PlaceCardsOnPlaceHolders();
    }

    void PlaceCardsOnPlaceHolders()
    {
        for (int i = 0; i < cardPlaceHolders.Count; i++)
        {
            if (i < shopCards.Count)
            {
                GameObject obj = Instantiate(shopCardPrefab, cardPlaceHolders[i].transform.position, Quaternion.identity, cardPlaceHolders[i].transform);
                obj.GetComponent<CardDisplay>().cardData = shopCards[i];
                placeHolderCardText[i].text = shopCards[i].cardName.ToString();
            }
        }
    }
}
