using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public string effectName;
    public string effectDescription;

    public abstract void ApplyEffect();
}










