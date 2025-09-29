using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackJackDeckScript : MonoBehaviour
{
    public List<Card> AllCards = new List<Card>(); //list of all cards in deck

    //private int currentCardIndex = 0; //index of current card in deck

    [SerializeField] private int _startingHandSize = 2; //number of cards to draw at start
    [SerializeField] private int _maxHandSize; //max number of cards in deck
    [SerializeField] private int _currentHandSize; //min number of cards in deck

    private void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("CardData/BlackJackCards"); //load all cards from Resources/Cards folder

        AllCards.AddRange(cards); //add all cards to deck

        FieldManagerScript hand = GameObject.Find("BlackJackHandManager").GetComponent<FieldManagerScript>();
        for (int i = 0; i < _startingHandSize; i++) //draw 8 cards at start
        {
            DrawCard(hand);
        }
    }

    public void DrawCard(FieldManagerScript blackJackHandManagerScript)
    {
        if (AllCards.Count == 0 || _currentHandSize == _maxHandSize)
            return;

        Card nextCard = AllCards[Random.Range(0, AllCards.Count - 1)];
        blackJackHandManagerScript.AddCardToField(nextCard);
        //currentCardIndex = (currentCardIndex + 1) % allCards.Count; //increment index and wrap around if needed
        _currentHandSize++;
    }
}
