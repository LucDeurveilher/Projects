using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitIconUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Trait trait;

    [SerializeField]Image icon;
    [SerializeField] GameObject infos;

    [SerializeField] TextMeshProUGUI traitName;
    [SerializeField] TextMeshProUGUI traitDescription;
    [SerializeField] TextMeshProUGUI traitPalier;

    // Start is called before the first frame update
    void Start()
    {
        SetIcon();
        SetInfos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetIcon()
    {
        icon.sprite = trait.traitIcon;
    }

    void SetInfos()
    {
        traitName.text = trait.name;
        traitDescription.text = trait.description;
        traitPalier.text = SetPalierText();
    }

    string SetPalierText()
    {
        string finalString = "";

        for (int i = 0; i < trait.palier.Count; i++)
        {
            finalString += Palier(trait.palier[i], trait.multiplicator[i]);
        }

        return finalString;
    }

    string Palier(int palier, float multiplicator)
    {
        return $"{palier}/  +{multiplicator}% of {trait.bonusModif}.\n";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infos.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infos.SetActive(false);
    }
}
