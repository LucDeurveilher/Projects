using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab; // assign card prefab in inspector
    public Transform handTransform; //Root of the hand position
    public Transform AIhandTransform; //Root of the hand position

    public int maxHandSize;

    public float fanSpread = 8f;
    public float cardSpacing = -100f;
    public float verticalSpacing = 100f;

    public List<GameObject> cardsInPlayerHand = new List<GameObject>(); //Hold a list of the card objects in our hands
    public List<GameObject> cardsInAIHand = new List<GameObject>(); //Hold a list of the card objects in our hands


    void Start()
    {

    }

    private void Update()
    {
        //UpdateHandVisuals();
    }

    public void BattleSetup(int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;
    }

    public void AddCardToHand(Card cardData)
    {
        //Instanciate the card
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInPlayerHand.Add(newCard);

        //Set the cardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().UpdateCardDisplay();

        UpdateHandVisuals();
    }

    public void AddCardToAIHand(Card cardData)
    {
        //Instanciate the card
        GameObject newCard = Instantiate(cardPrefab, AIhandTransform.position, Quaternion.identity, AIhandTransform);
        cardsInAIHand.Add(newCard);

        //Set the cardData of the instantiated card
        newCard.GetComponent<CardDisplay>().cardData = cardData;
        newCard.GetComponent<CardDisplay>().UpdateCardDisplay();

        //UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInPlayerHand.Count;

        if (cardCount == 1)
        {
            cardsInPlayerHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInPlayerHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);

            return;
        }

        for (int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInPlayerHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float horizontalOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //normalized card position between -1 , 1

            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            //Set card position
            cardsInPlayerHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0f);
        }
    }

}
