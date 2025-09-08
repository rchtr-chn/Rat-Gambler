using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DrawOneCardEffect", menuName = "Card Effects/Draw One Card")]
public class DrawOneCard : CardEffect
{
    public override void ApplyEffect()
    {
        DeckManagerScript deckManager = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        deckManager.DrawCardToHand(handManager);

        GameManagerScript gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gameManager.EndPlayerTurn();
    }
}
