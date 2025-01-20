using System;
using UnityEngine;
using static Card;

public class SpellEffectApplier
{
    public static void ApplySpell(Spell spell, CharacterStats targetStats)
    {
        for (int i = 0; i < spell.attributeTarget.Count; i++)
        {
            int changeAmount = spell.attributeChangeAmount.Count > i ? spell.attributeChangeAmount[i] : 0;
            ApplyEffectToAttribute(spell, spell.spellType, spell.attributeTarget[i], changeAmount, targetStats);
        }

        targetStats.healthUI.UpdateUi(targetStats.health,targetStats.maxHealth);
    }

    private static void ApplyEffectToAttribute(Spell spell, SpellType spellType, AttributeTarget attributeTarget, int changeAmount, CharacterStats targetStats)
    {
        int finalChangeAmount = spellType == SpellType.Buff ? changeAmount : -changeAmount;

        switch (attributeTarget)
        {
            case AttributeTarget.health:
                targetStats.health += finalChangeAmount;
                break;
            case AttributeTarget.dodgeChance:
                targetStats.dodgeChance += finalChangeAmount;
                break;
            case AttributeTarget.damage:
                targetStats.damageMin += finalChangeAmount;
                targetStats.damageMax += finalChangeAmount;
                break;
            case AttributeTarget.critChance:
                targetStats.critChance += finalChangeAmount;
                break;
            case AttributeTarget.critDamage:
                targetStats.critDamage += finalChangeAmount;
                break;
            case AttributeTarget.attackPattern:
                targetStats.attackPattern = spell.attackPatternToChangeTo;
                break;
            case AttributeTarget.damageType:
                targetStats.damageType[0] = spell.damageTypeToChangeTo;
                //ou Add ? plus tard
                break;
            case AttributeTarget.effectBoost:
                targetStats.effectBoost += finalChangeAmount;
                break;
            case AttributeTarget.cardType:
                targetStats.cardElementType.Add(spell.cardTypeToChangeTo);
                break;
            case AttributeTarget.priorityTarget:
                targetStats.priorityTarget = spell.priorityTargetToChangeTo;
                break;
            default:
                System.Diagnostics.Debug.WriteLine("Attribute target not implemented.");
                break;
        }

        ClampCharacterStats(targetStats);
    }

    private static void ClampCharacterStats(CharacterStats stats)
    {
        stats.health = Mathf.Max(stats.health, 0);
        stats.damageMin = Mathf.Max(stats.damageMin, 0);
        stats.damageMax = Mathf.Max(stats.damageMax, stats.damageMin);
        stats.dodgeChance = Mathf.Max(stats.dodgeChance, 0);
        stats.critChance = Mathf.Max(stats.critChance, 0);
        stats.critDamage = Mathf.Max(stats.critDamage, 0);
        stats.effectBoost = Mathf.Max(stats.effectBoost, 0);
        //stats.range = Mathf.Max(stats.range, 1);//ya plus
    }
}
