using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitUi : MonoBehaviour
{
    [SerializeField] GameObject traitPrefab;
    [SerializeField] VerticalLayoutGroup verticalLayoutGroup;

    private Dictionary<TraitStats, GameObject> traitsIcons = new Dictionary<TraitStats, GameObject>();

    public void AddUITrait(TraitStats trait)
    {
        GameObject uiTrait = Instantiate(traitPrefab, verticalLayoutGroup.transform);
        uiTrait.GetComponent<TraitIconUi>().traitStats = trait;

        traitsIcons.Add(trait, uiTrait);
    }

    public void RemoveUITrait(TraitStats trait)
    {
        if (traitsIcons.ContainsKey(trait))
        {
            Destroy(traitsIcons[trait]);
            traitsIcons.Remove(trait);
        }
    }
}
