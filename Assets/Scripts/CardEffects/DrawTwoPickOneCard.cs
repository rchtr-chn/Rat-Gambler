using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DrawTwoPickOneCardEffect", menuName = "Card Effects/Draw Two Pick One Card")]
public class DrawTwoPickOneCard : CardEffect
{
    public override void ApplyEffect()
    {
        DeckManagerScript deckManager = GameObject.FindGameObjectWithTag("PlayerDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript handManager = GameObject.Find("PlayerHandManager").GetComponent<HandManagerScript>();
        

        DeckManagerScript enemyDeckManager = GameObject.FindGameObjectWithTag("EnemyDeckManager").GetComponent<DeckManagerScript>();
        HandManagerScript enemyHandManager = GameObject.Find("EnemyHandManager").GetComponent<HandManagerScript>();
        
        for(int i = 0; i < 2; i++)
        {
            deckManager.DrawCardToHand(handManager);
            enemyDeckManager.DrawCardToHand(enemyHandManager);
        }

        GameManagerScript gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gameManager.EndPlayerTurn();
    }
}
