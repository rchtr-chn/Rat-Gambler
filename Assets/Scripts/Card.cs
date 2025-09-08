using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class Card : ScriptableObject
{
    public Sprite cardImage;
    public string cardName;
    public List<CardType> cardType;
    public int cardPoints;
    public bool isPlayable;

    public CardEffect cardEffect;

    public enum CardType
    {
        Aces, Poker, Power
    }
}
