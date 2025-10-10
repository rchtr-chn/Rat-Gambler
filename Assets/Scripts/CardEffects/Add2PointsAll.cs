using UnityEngine;


[CreateAssetMenu(fileName = "Add2PointsAllEffect", menuName = "Card Effects/Add 2 Points All")]
public class Add2PointsAll : CardEffect
{
    private GameManagerScript GameManager;
    public override void ApplyEffect(Card t)
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        GameManager.AdditionalEnemyPoints += 2;
        GameManager.AdditionalPlayerPoints += 2;

        GameManager.PlayerPlayedCard(t);
    }
}
