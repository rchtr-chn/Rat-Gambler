using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Add3PointsAnyEffect", menuName = "Card Effects/Add 3 Points Any")]
public class Add3PointsAny : CardEffect
{
    EffectManager effectManager;
    public override void ApplyEffect()
    {
        Debug.Log("Adding 3 points to any player...");
        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        effectManager.StartCoroutine(effectManager.Add3PointsAnyCoroutine());
    }
}
