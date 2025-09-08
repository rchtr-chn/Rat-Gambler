using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerScript : MonoBehaviour
{
    public List<Card> resourceDeck = new List<Card>(); //list of all cards in deck
    public List<Card> playingDeck = new List<Card>(); //list of all cards in deck

    //private int currentCardIndex = 0; //index of current card in deck

    [SerializeField] int startingHandSize = 3; //number of cards to draw at start
    [SerializeField] int maxHandSize; //max number of cards in deck
    public int currentHandSize; //min number of cards in deck\

    public Coroutine handInitializationCoroutine;
    public Coroutine drawCoroutine;

    public AudioManagerScript audioManager;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManagerScript>();
        Card[] cards;
        if (gameObject.CompareTag("EnemyDeckManager"))
        {
            cards = Resources.LoadAll<Card>("CardData/RatmiCards"); //load all cards from Resources/Cards folder
        }
        else
        {
            cards = Resources.LoadAll<Card>("CardData/RatmiCards");
        }

        resourceDeck.AddRange(cards); //add all cards to deck
        for(int i=0; i<8; i++)
        {
            int randomIndex = Random.Range(0, resourceDeck.Count);
            playingDeck.Add(resourceDeck[randomIndex]);
        }
    }

    public IEnumerator InitializeHand()
    {
        HandManagerScript hand;
        FieldManagerScript field;

        if (gameObject.CompareTag("EnemyDeckManager"))
        {
            hand = GameObject.Find("EnemyHandManager").GetComponent<HandManagerScript>();
            field = GameObject.Find("EnemyFieldManager").GetComponent<FieldManagerScript>();
        }
        else
        {
            hand = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
            field = GameObject.Find("PlayerFieldManager").GetComponent<FieldManagerScript>();
        }

        drawCoroutine = StartCoroutine(DrawUntilPoker(field));

        while (currentHandSize <= startingHandSize)
        {
            DrawCardToHand(hand);
            yield return null;
        }
        //DrawCardToField(field); //draw initial card to field
        handInitializationCoroutine = null;

        Debug.Log(hand.onHandCards.Count);
        Debug.Log(field.fieldCards.Count);
    }

    public void DrawCardToHand(HandManagerScript handManagerScript)
    {
        audioManager.PlaySfx(audioManager.drawCard);

        int randomIndex = Random.Range(0, playingDeck.Count - 1);
        if (playingDeck.Count == 0 || currentHandSize == maxHandSize)
            return;

        Card nextCard = playingDeck[randomIndex];
        handManagerScript.AddCardToHand(nextCard);
        currentHandSize++;
        playingDeck.RemoveAt(randomIndex); //remove drawn card from deck
    }
    IEnumerator DrawUntilPoker(FieldManagerScript fieldManagerScript)
    {
        if (playingDeck.Count > 0)
        {
            Card potentialCard = playingDeck[0];
            while (!potentialCard.cardType.Contains(Card.CardType.Poker))
            {
                potentialCard = playingDeck[Random.Range(0, playingDeck.Count - 1)];
                yield return null;
            }
            audioManager.PlaySfx(audioManager.drawCard);
            fieldManagerScript.AddCardToField(potentialCard);
            currentHandSize++;
            playingDeck.Remove(potentialCard); //remove drawn card from deck
        }

        drawCoroutine = null;
        yield return null;

    }

    public void DrawCardToField(FieldManagerScript fieldManagerScript)
    {
        int randomIndex = Random.Range(0, playingDeck.Count - 1);
        if (playingDeck.Count == 0 || currentHandSize == maxHandSize)
            return;

        Card nextCard = playingDeck[randomIndex];
        fieldManagerScript.AddCardToField(nextCard);
        currentHandSize++;
        playingDeck.RemoveAt(randomIndex); //remove drawn card from deck
    }

    public void DrawEntireDeck()
    {
        HandManagerScript hand = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();

        int index = playingDeck.Count;
        while (index > 0)
        {
            DrawCardToHand(hand);
            index--;
        }
    }
}
