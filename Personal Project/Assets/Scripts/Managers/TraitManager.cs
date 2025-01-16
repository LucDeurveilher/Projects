using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    GridManager gridManager;
    List<Trait> traits = new();

    List<TraitStats> activatedTraits = new();

    public List<CharacterStats> allies = new List<CharacterStats>();
    public List<CharacterStats> ennemies = new List<CharacterStats>();

    [SerializeField] TraitUi TraitUi;

    public static Action OnGridChange;

    // Start is called before the first frame update
    void Start()
    {
        //Load all card asset from the Resouces folder
        Trait[] ResourceTraits = Resources.LoadAll<Trait>("Traits");

        foreach (Trait trait in ResourceTraits)
        {
            traits.Add(trait);
        }

        gridManager = FindObjectOfType<GridManager>();
        ResetTraits();

        OnGridChange += UpdateTraits;
    }

    private void SetAlliesEnnemies()
    {
        allies.Clear();
        ennemies.Clear();

        foreach (GameObject obj in gridManager.gridObjects)
        {
            CharacterStats temp = obj.GetComponent<CharacterStats>();

            if (temp.isAllie)
            {
                allies.Add(temp);
            }
            else
            {
                ennemies.Add(temp);
            }
        }
    }

    void UpdateTraits()
    {
        StartCoroutine(Utility.PlayFonctionAfterTimer(0.1f, () => CheckTraits()));//Let CharacterStat Set
    }

    void ResetTraits()
    {
        foreach (TraitStats trait in activatedTraits)
        {
            trait.ResetTrait();
        }
    }

    void CheckTraits()
    {
        SetAlliesEnnemies();
        ResetTraits();

        foreach (Trait trait in traits)
        {
            int numActivation = 0;

            foreach (CharacterStats stats in allies)
            {

                foreach (Card.CardType element in stats.cardElementType)
                {
                    if (trait.elementType == element)
                    {
                        numActivation++;

                        TraitStats traitStats = FindTraitStat(trait);

                        if (traitStats == null)
                        {
                            TraitStats newTraitStat = new TraitStats();
                            newTraitStat.SetTrait(trait);

                            newTraitStat.AddCharacterStat(stats);

                            activatedTraits.Add(newTraitStat);

                            TraitUi.AddUITrait(trait);
                        }
                        else
                        {
                            traitStats.AddCharacterStat(stats);
                        }

                    }
                }

            }

            if (numActivation <= 0)
            {
                activatedTraits.Remove(FindTraitStat(trait));
                TraitUi.RemoveUITrait(trait);
            }
        }

        ActiveBoostTraits();
    }

    void ActiveBoostTraits()
    {
        foreach (TraitStats traitStats in activatedTraits)
        {
            traitStats.Boost(allies);
        }
    }

    TraitStats FindTraitStat(Trait trait)
    {
        foreach (TraitStats stats in activatedTraits)
        {
            if (stats.trait == trait)
            {
                return stats;
            }
        }

        return null;
    }

}
