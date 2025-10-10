using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public string EffectName;
    public string EffectDescription;

    public abstract void ApplyEffect(Card target);
}










