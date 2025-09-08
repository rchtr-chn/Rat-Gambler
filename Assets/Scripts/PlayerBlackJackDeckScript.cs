using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackJackDeckScript : MonoBehaviour
{
    public List<Card> allCards = new List<Card>(); //list of all cards in deck

    //private int currentCardIndex = 0; //index of current card in deck

    [SerializeField] int startingHandSize = 2; //number of cards to draw at start
    [SerializeField] int maxHandSize; //max number of cards in deck
    [SerializeField] int currentHandSize; //min number of cards in deck

    private void Start()
    {
        Card[] cards = Resources.LoadAll<Card>("CardData/BlackJackCards"); //load all cards from Resources/Cards folder

        allCards.AddRange(cards); //add all cards to deck

        FieldManagerScript hand = GameObject.Find("BlackJackHandManager").GetComponent<FieldManagerScript>();
        for (int i = 0; i < startingHandSize; i++) //draw 8 cards at start
        {
            DrawCard(hand);
        }
    }

    public void DrawCard(FieldManagerScript blackJackHandManagerScript)
    {
        if (allCards.Count == 0 || currentHandSize == maxHandSize)
            return;

        Card nextCard = allCards[Random.Range(0, allCards.Count - 1)];
        blackJackHandManagerScript.AddCardToField(nextCard);
        //currentCardIndex = (currentCardIndex + 1) % allCards.Count; //increment index and wrap around if needed
        currentHandSize++;
    }
}
