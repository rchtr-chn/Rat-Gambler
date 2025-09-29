using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class Card : ScriptableObject
{
    public Sprite CardImage;
    public string CardName;
    public List<CardTypes> CardType;
    public int CardPoints;
    public bool IsPlayable;

    public CardEffect CardEffect;

    public enum CardTypes
    {
        Aces, Poker, Power
    }
}
