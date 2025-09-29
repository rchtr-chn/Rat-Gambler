using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManagerScript : MonoBehaviour
{
    public DeckManagerScript DeckManagerScript;
    public HandManagerScript HandManagerScript;
    public Text[] PlaceHolderCardText;
    private List<Card> _shopCards = new List<Card>();
    public GameObject ShopCardPrefab;

    public AudioManagerScript AudioManager;

    public List<GameObject> CardPlaceHolders = new List<GameObject>();

    private void Start()
    {
        AudioManager = FindObjectOfType<AudioManagerScript>();
        AudioManager.MusicSource.clip = AudioManager.LevelSelectBGM;
        AudioManager.MusicSource.loop = true;
        AudioManager.MusicSource.Play();

        DeckManagerScript = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();

        //set game manager to player turn so cards can be dragged
        GameManagerScript gameManager = FindObjectOfType<GameManagerScript>();
        gameManager.IsPlayerTurn = true;

        ShowPlayerDeck();

        GetRandomCards();
    }

    void ShowPlayerDeck()
    {
        for(int i = 0; i < HandManagerScript.OnHandCards.Count - 1; i++)
        {
            HandManagerScript.OnHandCards[i].GetComponent<CardMovementScript>().CardPlay = new Vector2(0, 1200);
        }

        DeckManagerScript.DrawEntireDeck();
    }

    void GetRandomCards()
    {
        Card[] cards = Resources.LoadAll<Card>("CardData/ShopCards");
        for (int i = 0; i < 4; i++)
        {
            int randomIndex = Random.Range(0, cards.Length - 1);
            _shopCards.Add(cards[randomIndex]);
        }

        PlaceCardsOnPlaceHolders();
    }

    void PlaceCardsOnPlaceHolders()
    {
        for (int i = 0; i < CardPlaceHolders.Count; i++)
        {
            if (i < _shopCards.Count)
            {
                GameObject obj = Instantiate(ShopCardPrefab, CardPlaceHolders[i].transform.position, Quaternion.identity, CardPlaceHolders[i].transform);
                obj.GetComponent<CardDisplay>().CardData = _shopCards[i];
                PlaceHolderCardText[i].text = _shopCards[i].CardName.ToString();
            }
        }
    }
}
