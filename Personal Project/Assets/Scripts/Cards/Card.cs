using System.Collections.Generic;
using UnityEngine;


public class Card : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
    public List<CardType> cardType;
    public string description;

    public enum CardType
    {
        Fire,
        Earth,
        Water,
        Dark,
        Light,
        Air
    }

    public enum DamageType
    {
        None,
        Bleed,
        Poison,
        Freeze,
    }

    public enum AttackPattern
    {
        Single,
        MultiTarget,
        Cross,
        Column,
        Row,
        TwoByTwo,
        FourByFour
    }

    public enum PriorityTarget
    {
        Close,
        Far,
        LeastCurrentHealth,
        MostCurrentHealth,
    }

    public enum SpellType
    {
        Buff,
        Debuff
    }

    public enum AttributeTarget
    {
        health,
        damage,
        range,
        attackPattern,
        damageType,
        cardType,
        priorityTarget
    }
}
