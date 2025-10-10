using UnityEngine;


[CreateAssetMenu(fileName = "Minus3PointsAnyEffect", menuName = "Card Effects/Minus 3 Points Any")]
public class Minus3PointsAny : CardEffect
{
    private EffectManager _effectManager;
    public override void ApplyEffect(Card t)
    {
        _effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        _effectManager.StartCoroutine(_effectManager.Minus3PointsAnyCoroutine(t));

    }
}
