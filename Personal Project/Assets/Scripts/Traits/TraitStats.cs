using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Trait;

public class TraitStats
{
    public Trait trait;

    [SerializeField]
    public List<CharacterStats> characters = new();

    private Card.CardType elementType;

    [SerializeField]
    string bonus;

    [SerializeField]
    List<int> palier = new List<int>();

    [SerializeField]
    public List<float> multiplicator = new List<float>();

    private BoostTarget boostTarget;

    private List<CharacterStats> alreadyBoosted = new();

    private Dictionary<CharacterStats, float> lastValuesBoost = new Dictionary<CharacterStats, float>();
    struct ValueStat
    {
        public int min;
        public int max;
    }

    private Dictionary<CharacterStats, ValueStat> beforeBoostValuesStats = new Dictionary<CharacterStats, ValueStat>();

    public void SetTrait(Trait Trait)
    {
        trait = Trait;

        elementType = trait.elementType;

        foreach (int tempPalier in trait.palier)
        {
            int temp = tempPalier;
            palier.Add(temp);
        }

        foreach (float tempMultiplicator in trait.multiplicator)
        {
            float temp = tempMultiplicator;
            multiplicator.Add(temp);
        }

        boostTarget = trait.boostTarget;
    }

    public void Boost(List<CharacterStats> characterStats)
    {
        int number = characters.Count;
        float boostValue = 1.0f;

        if (number < palier[0])
        {
            return;
        }

        for (int i = 0; i < palier.Count; i++)
        {
            if (number == palier[i])
            {
                boostValue = multiplicator[i];
            }
        }

        if (number > palier[palier.Count - 1])
        {
            boostValue = multiplicator[multiplicator.Count - 1];
        }

        switch (boostTarget)
        {
            case BoostTarget.health:
                BoostHealth(characterStats, boostValue);
                break;
            case BoostTarget.damage:
                BoostDamage(characterStats, boostValue);
                break;
            default:
                break;
        }
    }

    private void BoostHealth(List<CharacterStats> characterStats, float boostValue)
    {
        //Debug.Log($"Boost value = health * {boostValue}");

        foreach (CharacterStats character in characterStats)
        {
            if (!alreadyBoosted.Contains(character))//if not countain the character so boost him
            {
                //Debug.Log($"character {character} boosted! health before = {character.health}");

                ValueStat valueStats;
                valueStats.min = character.health;
                valueStats.max = character.health;

                beforeBoostValuesStats.Add(character, valueStats);
                lastValuesBoost.Add(character, boostValue);

                float temp = (float)character.health * boostValue;
                character.health = (int)temp;

                //Debug.Log($"health after = {character.health}");
                alreadyBoosted.Add(character);
            }
            else if (lastValuesBoost[character] != boostValue)//adjust if the value is higher
            {
                float temp = (float)beforeBoostValuesStats[character].min * boostValue;
                character.health = (int)temp;

                lastValuesBoost[character] = boostValue;

                Debug.Log("adjustment");
            }

        }
    }

    private void BoostDamage(List<CharacterStats> characterStats, float boostValue)
    {
        //Debug.Log($"Boost value = damage * {boostValue}");


        foreach (CharacterStats character in characterStats)
        {
            if (!alreadyBoosted.Contains(character))//if not countain the character so boost him
            {
                //Debug.Log($"character {character} boosted! damage before = {character.damageMin} - {character.damageMax}");

                ValueStat valueStats;
                valueStats.min = character.damageMin;
                valueStats.max = character.damageMax;

                beforeBoostValuesStats.Add(character, valueStats);
                lastValuesBoost.Add(character, boostValue);

                float temp = (float)character.damageMin * boostValue;
                character.damageMin = (int)temp;

                temp = (float)character.damageMax * boostValue;
                character.damageMax = (int)temp;

                //Debug.Log($"damage after = {character.damageMin} - {character.damageMax}");
                alreadyBoosted.Add(character);
            }
            else if (lastValuesBoost[character] != boostValue)//adjust if the value is higher
            {
                float temp = (float)beforeBoostValuesStats[character].min * boostValue;
                character.damageMin = (int)temp;

                temp = (float)(float)beforeBoostValuesStats[character].max * boostValue;
                character.damageMax = (int)temp;

                lastValuesBoost[character] = boostValue;

                Debug.Log("adjustment");
            }

        }
    }

    public void AddCharacterStat(CharacterStats characterStats)
    {
        characters.Add(characterStats);
    }

    public void ResetTrait()
    {
        characters.Clear();
    }
}
