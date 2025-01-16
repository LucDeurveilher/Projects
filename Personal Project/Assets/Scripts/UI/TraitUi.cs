using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUi : MonoBehaviour
{
    [SerializeField] GameObject traitPrefab;
    [SerializeField] VerticalLayoutGroup verticalLayoutGroup;

    private Dictionary<Trait, GameObject> traitsIcons = new Dictionary<Trait, GameObject>();

    public void AddUITrait(Trait trait)
    {
        GameObject uiTrait = Instantiate(traitPrefab, verticalLayoutGroup.transform);
        uiTrait.GetComponent<TraitIconUi>().trait = trait;

        traitsIcons.Add(trait, uiTrait);
    }

    public void RemoveUITrait(Trait trait)
    {
        if (traitsIcons.ContainsKey(trait))
        {
            Destroy(traitsIcons[trait]);
            traitsIcons.Remove(trait);
        }
    }
}
