using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public List<Card> allCards = new List<Card>();


    [SerializeField] public int startingHandSize = 6;

    public int maxHandSize;
    public int currentHandSize = 6 ;

    private HandManager handManager;
    private DrawPileManager drawPileManager;

    private bool startBattleRun = false;

    private void Start()
    {
        //Load all card asset from the Resouces folder
        Card[] cards = Resources.LoadAll<Card>("Cards");

        //Add the loaded cards to the allCards list
        allCards.AddRange(cards);
    }

    private void Awake()
    {
        if (drawPileManager == null)
        {
            drawPileManager = FindObjectOfType<DrawPileManager>();
        }

        if (handManager == null)
        {
            handManager = FindObjectOfType<HandManager>();
        }
    }

    private void Update()
    {
        if (startBattleRun)
        {
            BattleSetup();
        }
    }

    public void BattleSetup()
    {
        handManager.BattleSetup(maxHandSize);
        drawPileManager.MakeDrawPile(allCards);
        drawPileManager.BattleSetup(startingHandSize, maxHandSize);
        startBattleRun = false;
    }

    //Create a battle if not already set or do nothing 
    public void ContinueBattle()
    {
        if (!GameManager.Instance.BattleSet)
        {
            NewBattle();
        }
    }

    public void NewBattle()
    {
        GameManager.Instance.BattleSet = true;
        startBattleRun = true;
    }
}
