using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Trait", menuName = "Traits")]
public class Trait : ScriptableObject
{
    public enum BoostTarget
    {
        health,
        dodgeChance,
        damage,
        critChance,
        critDamage,
        effectBoost
    }
    [SerializeField] public Sprite traitIcon;

    [SerializeField]
    public Card.CardType elementType;

    [SerializeField]
    public string description;

    [SerializeField]
    public string bonusModif;

    [SerializeField]
    public List<int> palier = new List<int>();

    [SerializeField]
    public List<int> multiplicator = new List<int>();

    [SerializeField]
    public BoostTarget boostTarget;
}
