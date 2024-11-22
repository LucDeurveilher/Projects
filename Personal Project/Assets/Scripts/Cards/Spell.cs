using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell Card", menuName = "Card/Spell")]
public class Spell : Card
{
    public SpellType spellType;
    public List<AttributeTarget> attributeTarget;
    public List<int> attributeChangeAmount;

    public AttackPattern attackPatternToChangeTo;
    public DamageType damageTypeToChangeTo;
    public CardType cardTypeToChangeTo;
    public PriorityTarget priorityTargetToChangeTo;
}
