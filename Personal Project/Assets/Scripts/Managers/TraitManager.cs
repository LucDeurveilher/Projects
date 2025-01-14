using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TraitManager : MonoBehaviour
{
    GridManager gridManager;
    List<Trait> traits = new();

    List<Trait> activatedTraits = new();

    public List<CharacterStats> allies = new List<CharacterStats>();
    public List<CharacterStats> ennemies = new List<CharacterStats>();

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

        OnGridChange += CheckTraits;
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

    void ResetTraits()
    {
        activatedTraits.Clear();
        foreach (Trait trait in traits)
        {
            trait.number = 0;
        }
    }

    void CheckTraits()
    {
        SetAlliesEnnemies();
        ResetTraits();

        foreach (CharacterStats stats in allies)
        {
            foreach (Trait trait in traits)
            {
                if (trait.characters.Contains(stats.characterStartData))
                {
                    Debug.Log("trait detected");
                    trait.number++;

                    activatedTraits.Add(trait);
                }
                Debug.Log(stats.characterStartData);
            }
        }

        ActiveBoostTraits();
    }

    void ActiveBoostTraits()
    {
        foreach(Trait trait in activatedTraits)
        {
            foreach(CharacterStats stats in allies)
            {
                float temp = (float)stats.health * 1.5f;
                stats.health = (int)temp;
            }
        }
    }

}
