using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Trait", menuName = "Traits")]
public class Trait : ScriptableObject
{
    public enum BoostTarget
    {
        health,
        damage
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
    public List<float> multiplicator = new List<float>();

    [SerializeField]
    public BoostTarget boostTarget;
}
