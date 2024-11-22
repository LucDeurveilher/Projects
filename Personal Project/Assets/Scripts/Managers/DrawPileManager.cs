using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPileManager : MonoBehaviour
{
    public List<Card> drawPile = new List<Card>();

    [SerializeField] public int startingHandSize = 6;
    private int currentIndex = 0;
    public int maxHandSize;
    public int currentPlayerHandSize;
    public int currentAIHandSize;

    private HandManager handManager;
    private DiscardManager discardManager;

    public TextMeshProUGUI drawPileCounter;

    private void Start()
    {
        handManager = FindObjectOfType<HandManager>();
    }

    private void Update()
    {
        if (handManager != null)
        {
            currentPlayerHandSize = handManager.cardsInPlayerHand.Count;
            currentAIHandSize = handManager.cardsInAIHand.Count;
        }
    }

    public void MakeDrawPile(List<Card> cardsToAdd)
    {
        drawPile.AddRange(cardsToAdd);
        Utility.Shuffle(drawPile);
        UpdateDrawPileCount();
    }

    public void BattleSetup(int numberOffCardsToDraw, int setMaxHandSize)
    {
        maxHandSize = setMaxHandSize;

        for (int i = 0;i <numberOffCardsToDraw;i++)
        {
            DrawCard(handManager);
            DrawCardAI(handManager);
        }
    }

    public void DrawCard(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            RefillDeckFromDiscard();
        }

        if (currentPlayerHandSize < maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToHand(nextCard);
            drawPile.RemoveAt(currentIndex);

            UpdateDrawPileCount();
            if (drawPile.Count > 0)
            {
                currentIndex %= drawPile.Count;
            }
        }
    }

    public void DrawCardAI(HandManager handManager)
    {
        if (drawPile.Count == 0)
        {
            RefillDeckFromDiscard();
        }

        if (currentAIHandSize < maxHandSize)
        {
            Card nextCard = drawPile[currentIndex];
            handManager.AddCardToAIHand(nextCard);
            drawPile.RemoveAt(currentIndex);

            UpdateDrawPileCount();
            if (drawPile.Count > 0)
            {
                currentIndex %= drawPile.Count;
            }
        }
    }

    private void RefillDeckFromDiscard()
    {
        if (discardManager == null)
        {
            discardManager = FindObjectOfType<DiscardManager>();
        }

        if (discardManager != null && discardManager.discardCardsCount > 0)
        {
            drawPile = discardManager.PullAllFromDiscard();
            Utility.Shuffle(drawPile);
            currentIndex = 0;
        }
    }

    private void UpdateDrawPileCount()
    {
        drawPileCounter.text = drawPile.Count.ToString();
    }


}
