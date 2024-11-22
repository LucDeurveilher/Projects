using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CardDisplay : MonoBehaviour
{
    //All cards elements
    public Card cardData;
    public Image cardImage;
    public TMP_Text nameText;
    public Image[] typeImages;
    public Image displayImage;

    public GameObject characterElements;
    public GameObject spellElements;

    public GameObject characterCardLabel;
    public GameObject spellCardLabel;

    public TMP_Text descriptionText;

    [Header("Character")]
    //Character card elements
    public TMP_Text healthText;
    public TMP_Text damageText;
    public GameObject[] attackPattern;
    public GameObject[] priorityTarget;

    [Header("Spell")]
    //Spell card elements
    public GameObject[] spellTypeLabels;
    public GameObject[] attributeTargetSymbols;
    public float attributeSymbolSpacing = 10f;
    public TMP_Text attributeChangeAmountText;

    public Color[] typeColors =
    {
        Color.red, // Fire
            new Color(0.8f,0.52f,0.24f),//Earth
            Color.blue, //Water
            Color.magenta, //Dark
            Color.yellow, // Light
            Color.cyan // air
    };

    private void Update()
    {
        //UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        //All card changes
        //cardImage.color = typeColors[(int)cardData.cardType[0]];
        nameText.text = cardData.cardName;
        displayImage.sprite = cardData.cardSprite;
        descriptionText.text = cardData.description;

        //Update type images
        for (int i = 0; i < cardData.cardType.Count; i++)
        {
            typeImages[i].gameObject.SetActive(true);
            typeImages[i].color = typeColors[(int)cardData.cardType[i]];
        }

        //Specifi card changes
        if (cardData is Character characterCard)
        {
            UpdateDisplayCharacterCard(characterCard);
        }
        else if (cardData is Spell spellCard)
        {
            UpdateDisplaySpellCard(spellCard);
        }
    }

    private void UpdateDisplayCharacterCard(Character characterCard)
    {
        spellElements.SetActive(false);
        characterElements.SetActive(true);
        characterCardLabel.SetActive(true);

        healthText.text = characterCard.health.ToString();
        damageText.text = $"{characterCard.damageMin} - {characterCard.damageMax}";

        attackPattern[(int)characterCard.attackPattern].SetActive(true);
        priorityTarget[(int)characterCard.priorityTarget].SetActive(true);
    }

    private void UpdateDisplaySpellCard(Spell spellCard)
    {
        spellElements.SetActive(true);
        characterElements.SetActive(false);
        spellCardLabel.SetActive(true);

        //Set correct spell type label
        foreach (GameObject label in spellTypeLabels)
        {
            label.SetActive(false);
        }
        spellTypeLabels[(int)spellCard.spellType].SetActive(true);

        //Reset and Update attribute target symbols
        foreach (GameObject symbol in attributeTargetSymbols)
        {
            symbol.SetActive(false);
        }

        for (int i = 0; i < spellCard.attributeTarget.Count; i++)
        {
            GameObject currentSymbol = attributeTargetSymbols[(int)spellCard.attributeTarget[i]];
            currentSymbol.SetActive(true);
            //float newYPosition = i * attributeSymbolSpacing;
            //currentSymbol.transform.localPosition = new Vector3(0, newYPosition, 0);
        }

        //Display attribute change amounts
        attributeChangeAmountText.text = string.Join(", ", spellCard.attributeChangeAmount);
    }

}
