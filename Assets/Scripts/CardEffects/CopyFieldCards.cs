using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CopyFieldCardsEffect", menuName = "Card Effects/Copy Field Card")]
public class CopyFieldCards : CardEffect
{
    EffectManager effectManager;
    public override void ApplyEffect()
    {
        effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        effectManager.StartCoroutine(effectManager.CopyCardEffect());
    }
}
