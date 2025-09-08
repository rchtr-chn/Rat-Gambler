using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Minus3PointsAnyEffect", menuName = "Card Effects/Minus 3 Points Any")]
public class Minus3PointsAny : CardEffect
{
    EffectManager effectManager;
    public override void ApplyEffect()
    {
        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        effectManager.StartCoroutine(effectManager.Minus3PointsAnyCoroutine());

    }
}
