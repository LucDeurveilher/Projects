using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TraitIconUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TraitStats traitStats;

    [SerializeField] Image icon;
    [SerializeField] GameObject infos;

    [SerializeField] TextMeshProUGUI traitName;
    [SerializeField] TextMeshProUGUI traitDescription;
    [SerializeField] TextMeshProUGUI traitPalier;
    [SerializeField] Transform iconCharacters;
    [SerializeField] GameObject prefabTraitCharacterIcon;

    bool imageSet = false;

    // Start is called before the first frame update
    private void OnEnable()
    {
        ;//bug
    }

    private void Start()
    {
        SetIcon();
        SetInfos();
    }

    // Update is called once per frame
    void Update()
    {
        if (!imageSet && iconCharacters.gameObject.activeInHierarchy)
        {
            SetCharactersImage();
            imageSet = true;
        }
        
        if (imageSet && !iconCharacters.gameObject.activeInHierarchy)
        {
            imageSet = false;
        }
    }

    void SetIcon()
    {
        icon.sprite = traitStats.trait.traitIcon;
    }

    void SetInfos()
    {
        traitName.text = traitStats.trait.name;
        traitDescription.text = traitStats.trait.description;
        traitPalier.text = SetPalierText();
    }

    string SetPalierText()
    {
        string finalString = "";

        for (int i = 0; i < traitStats.trait.palier.Count; i++)
        {
            finalString += Palier(traitStats.trait.palier[i], traitStats.multiplicator[i]);
        }

        return finalString;
    }

    string Palier(int palier, float multiplicator)
    {
        return $"{palier}/  +{multiplicator}% of {traitStats.trait.bonusModif}.\n";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        infos.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infos.SetActive(false);
    }

    public void SetCharactersImage()
    {
        //Debug.Log($"iconCharacters.childCount: {iconCharacters.childCount}");

        for (int i = iconCharacters.childCount - 1; i >= 0; i--)
        {
            //Debug.Log("destroy");
            Destroy(iconCharacters.GetChild(i).gameObject);
        }

        foreach (CharacterStats characterStats in traitStats.characters)
        {
            GameObject temp = Instantiate(prefabTraitCharacterIcon, iconCharacters);
            Image tempImage = temp.GetComponent<Image>();
            tempImage.sprite = characterStats.characterStartData.cardSprite;
        }
    }
}
