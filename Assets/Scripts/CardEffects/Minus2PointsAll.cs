using UnityEngine;

[CreateAssetMenu(fileName = "Minus2PointsAllEffect", menuName = "Card Effects/Minus 2 Points All")]
public class Minus2PointsAll : CardEffect
{
    private GameManagerScript _gameManager;
    public override void ApplyEffect()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        _gameManager.AdditionalEnemyPoints -= 2;
        _gameManager.AdditionalPlayerPoints -= 2;

        _gameManager.EndPlayerTurn();
    }
}
    