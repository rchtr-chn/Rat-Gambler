using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManagerScript : MonoBehaviour
{
    public List<Card> ResourceDeck = new List<Card>(); //list of all cards in deck
    public List<Card> PlayingDeck = new List<Card>(); //list of all cards in deck

    //private int currentCardIndex = 0; //index of current card in deck

    [SerializeField] private int _startingHandSize = 3; //number of cards to draw at start
    [SerializeField] private int _maxHandSize; //max number of cards in deck
    public int CurrentHandSize; //min number of cards in deck\

    public Coroutine HandInitializationCoroutine;
    public Coroutine DrawCoroutine;

    public AudioManagerScript AudioManager;

    private void Awake()
    {
        AudioManager = FindObjectOfType<AudioManagerScript>();
        Card[] cards;
        if (gameObject.CompareTag("EnemyDeckManager"))
        {
            cards = Resources.LoadAll<Card>("CardData/RatmiCards"); //load all cards from Resources/Cards folder
        }
        else
        {
            cards = Resources.LoadAll<Card>("CardData/RatmiCards");
        }

        ResourceDeck.AddRange(cards); //add all cards to deck
        for(int i=0; i<8; i++)
        {
            int randomIndex = Random.Range(0, ResourceDeck.Count);
            PlayingDeck.Add(ResourceDeck[randomIndex]);
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

        DrawCoroutine = StartCoroutine(DrawUntilPoker(field));

        while (CurrentHandSize <= _startingHandSize)
        {
            DrawCardToHand(hand);
            yield return null;
        }
        //DrawCardToField(field); //draw initial card to field
        HandInitializationCoroutine = null;

        Debug.Log(hand.OnHandCards.Count);
        Debug.Log(field.FieldCards.Count);
    }

    public void DrawCardToHand(HandManagerScript handManagerScript)
    {
        AudioManager.PlaySfx(AudioManager.DrawCard);

        int randomIndex = Random.Range(0, PlayingDeck.Count - 1);
        if (PlayingDeck.Count == 0 || CurrentHandSize == _maxHandSize)
            return;

        Card nextCard = PlayingDeck[randomIndex];
        handManagerScript.AddCardToHand(nextCard);
        CurrentHandSize++;
        PlayingDeck.RemoveAt(randomIndex); //remove drawn card from deck
    }
    IEnumerator DrawUntilPoker(FieldManagerScript fieldManagerScript)
    {
        if (PlayingDeck.Count > 0)
        {
            Card potentialCard = PlayingDeck[0];
            while (!potentialCard.CardType.Contains(Card.CardTypes.Poker))
            {
                potentialCard = PlayingDeck[Random.Range(0, PlayingDeck.Count - 1)];
                yield return null;
            }
            AudioManager.PlaySfx(AudioManager.DrawCard);
            fieldManagerScript.AddCardToField(potentialCard);
            CurrentHandSize++;
            PlayingDeck.Remove(potentialCard); //remove drawn card from deck
        }

        DrawCoroutine = null;
        yield return null;

    }

    public void DrawCardToField(FieldManagerScript fieldManagerScript)
    {
        int randomIndex = Random.Range(0, PlayingDeck.Count - 1);
        if (PlayingDeck.Count == 0 || CurrentHandSize == _maxHandSize)
            return;

        Card nextCard = PlayingDeck[randomIndex];
        fieldManagerScript.AddCardToField(nextCard);
        CurrentHandSize++;
        PlayingDeck.RemoveAt(randomIndex); //remove drawn card from deck
    }

    public void DrawEntireDeck()
    {
        HandManagerScript hand = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();

        int index = PlayingDeck.Count;
        while (index > 0)
        {
            DrawCardToHand(hand);
            index--;
        }
    }
}
