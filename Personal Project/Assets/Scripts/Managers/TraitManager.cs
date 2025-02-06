using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    GridManager gridManager;
    List<Trait> traits = new();

    List<TraitStats> activatedTraitsPlayer = new();
    public List<TraitStats> activatedTraitsAI = new();

    public List<CharacterStats> allies = new List<CharacterStats>();
    public List<CharacterStats> enemies = new List<CharacterStats>();

    [SerializeField] TraitUi TraitUiPlayer;
    [SerializeField] TraitUi TraitUiAI;

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
        GameManager.Instance.ResetGame += UpdateTraits;
    }

    private void SetAlliesEnnemies()
    {
        allies.Clear();
        enemies.Clear();

        foreach (GameObject obj in gridManager.gridObjects)
        {
            CharacterStats temp = obj.GetComponent<CharacterStats>();

            if (temp.isAllie)
            {
                allies.Add(temp);
            }
            else
            {
                enemies.Add(temp);
            }
        }
    }

    void UpdateTraits()
    {
        StartCoroutine(Utility.PlayFonctionAfterTimer(0.1f, () => CheckTraits()));//Let CharacterStat Set
    }

    void ResetTraits()
    {
        activatedTraitsPlayer.ForEach(trait => trait.ResetTrait());
        activatedTraitsAI.ForEach(trait => trait.ResetTrait());
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

                        TraitStats traitStats = FindTraitStatPlayer(trait);

                        if (traitStats == null)
                        {
                            TraitStats newTraitStat = new TraitStats();
                            newTraitStat.SetTrait(trait);

                            newTraitStat.AddCharacterStat(stats);

                            activatedTraitsPlayer.Add(newTraitStat);

                            TraitUiPlayer.AddUITrait(newTraitStat);
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
                TraitStats traitStats = FindTraitStatPlayer(trait);

                if (traitStats != null)
                {
                    activatedTraitsPlayer.Remove(traitStats);
                    TraitUiPlayer.RemoveUITrait(traitStats);
                }
            }

            numActivation = 0;

            foreach (CharacterStats stats in enemies)
            {

                foreach (Card.CardType element in stats.cardElementType)
                {
                    if (trait.elementType == element)
                    {
                        numActivation++;

                        TraitStats traitStats = FindTraitStatAI(trait);

                        if (traitStats == null)
                        {
                            TraitStats newTraitStat = new TraitStats();
                            newTraitStat.SetTrait(trait);

                            newTraitStat.AddCharacterStat(stats);

                            activatedTraitsAI.Add(newTraitStat);

                            TraitUiAI.AddUITrait(newTraitStat);
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
                TraitStats traitStats = FindTraitStatAI(trait);

                if (traitStats != null)
                {
                    activatedTraitsAI.Remove(traitStats);
                    TraitUiAI.RemoveUITrait(traitStats);
                }
            }
        }

        ActiveBoostTraits();
    }

    void ActiveBoostTraits()
    {
        activatedTraitsPlayer.ForEach(trait => trait.Boost(allies));
        activatedTraitsAI.ForEach(trait => trait.Boost(enemies));
    }

    TraitStats FindTraitStatPlayer(Trait trait)
    {
        foreach (TraitStats stats in activatedTraitsPlayer)
        {
            if (stats.trait == trait)
            {
                return stats;
            }
        }

        return null;
    }

    TraitStats FindTraitStatAI(Trait trait)
    {
        foreach (TraitStats stats in activatedTraitsAI)
        {
            if (stats.trait == trait)
            {
                return stats;
            }
        }

        return null;
    }

    public List<Trait> GetAllTraitsOfThisCharacter(Character character)
    {
        List<Trait> traitsCharacter = new List<Trait>();

        foreach (Trait trait in traits)
        {
            foreach (Card.CardType element in character.cardType)
            {
                if (trait.elementType == element && !traitsCharacter.Contains(trait))
                {
                    traitsCharacter.Add(trait);
                }
            }
        }

        return traits;
    }

}
