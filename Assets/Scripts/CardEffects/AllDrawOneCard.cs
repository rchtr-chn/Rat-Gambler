using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AllDrawOneCardEffect", menuName = "Card Effects/All Draw One Card")]
public class AllDrawOneCard : CardEffect
{
    public override void ApplyEffect()
    {
        DeckManagerScript deckManager = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        deckManager.DrawCardToHand(handManager);

        DeckManagerScript enemyDeckManager = GameObject.FindGameObjectWithTag("EnemyDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript enemyHandManager = GameObject.Find("EnemyHandManager").GetComponent<HandManagerScript>();
        enemyDeckManager.DrawCardToHand(enemyHandManager);

        GameManagerScript gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gameManager.EndPlayerTurn();
    }
}
