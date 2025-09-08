using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Minus2PointsAllEffect", menuName = "Card Effects/Minus 2 Points All")]
public class Minus2PointsAll : CardEffect
{
    GameManagerScript gameManager;
    public override void ApplyEffect()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        gameManager.additionalEnemyPoints -= 2;
        gameManager.additionalPlayerPoints -= 2;

        gameManager.EndPlayerTurn();
    }
}
    