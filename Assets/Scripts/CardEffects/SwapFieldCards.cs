using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwapFieldCardsEffect", menuName = "Card Effects/Swap Field Cards")]
public class SwapFieldCards : CardEffect
{
    public override void ApplyEffect()
    {
        Debug.Log("Swapping field cards...");
        //EffectManager effectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
        //effectManager.StartCoroutine(effectManager.SwapCards());
    }
}
