using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card", menuName = "Card/Character")]
public class Character : Card
{
    public int health;
    public int damageMin;
    public int damageMax;
    public List<DamageType> damageType;
    public GameObject prefab;
    public int range;
    public AttackPattern attackPattern;
    public PriorityTarget priorityTarget;

    public bool isAllie = false;
}