using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class AIBase : MonoBehaviour
{
    GridManager gridManager;
    HandManager handManager;
    DiscardManager discardManager;
    DrawPileManager drawPileManager;
    TurnManager turnManager;
    TraitManager traitManager;

    private Card cardData;
    private GameObject actualCart;

    public List<GameObject> allies = new List<GameObject>();
    public List<GameObject> ennemies = new List<GameObject>();

    public bool firstToPlay = true;
    bool firstBoardSet = false;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = FindObjectOfType<GridManager>();
        handManager = FindObjectOfType<HandManager>();
        discardManager = FindObjectOfType<DiscardManager>();
        drawPileManager = FindObjectOfType<DrawPileManager>();
        turnManager = FindObjectOfType<TurnManager>();
        traitManager = FindObjectOfType<TraitManager>();
        turnManager.AI = this;

        //What to do on Reset
        GameManager.Instance.ResetGame += () => 
        { 
            firstToPlay = true; 
            firstBoardSet = false; 
        };
    }

    IEnumerator WaitToEndTheTurn()
    {
        yield return new WaitForSeconds(1);
        turnManager.EndTurn();
        yield return null;
    }

    public void PlayAI()
    {
        AnalizeGrid();
    }

    IEnumerator PlaceCard()
    {
        foreach (GameObject card in cardToPlay.Keys)
        {
            actualCart = card;
            cardData = actualCart.GetComponent<CardDisplay>().cardData;

            if (cardData is Character characterCard)
            {
                TryToPlayCharacterCard(card, characterCard);
            }
            else if (cardData is Spell spellCard)
            {
                TryToPlaySpellCard(spellCard);
            }
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);
        turnManager.EndTurn();
        yield return null;
    }

    private void ChooseCard()
    {
        StartCoroutine(PlaceCard());
    }

    private void DrawCardAI()
    {
        drawPileManager.DrawCardAI(handManager);
        StartCoroutine(WaitToEndTheTurn());
    }

    private void TryToPlayCharacterCard(GameObject key, Character characterCard)
    {
        Vector2 pos = cardToPlay[key];

        GridCell cell = gridManager.gridCells[(int)pos.x, (int)pos.y].GetComponent<GridCell>();

        if (gridManager.AddObjectToGrid(characterCard.prefab, pos))
        {
            characterCard.isAllie = false;
            cell.objectInCell.GetComponent<CharacterStats>().characterStartData = characterCard;
            Utility.FlipSprite(cell.objectInCell, true, false);

            TraitManager.OnGridChange?.Invoke();

            handManager.cardsInAIHand.Remove(actualCart);
            discardManager.AddToDiscard(cardData);

            return;//have placed a card
        }
    }

    private bool IsPositionCellValid(Vector2 pos)
    {
        GridCell cell = gridManager.gridCells[(int)pos.x, (int)pos.y].GetComponent<GridCell>();

        if (cell != null && !cell.cellFull)
        {
            return true;
        }

        return false;
    }

    private void TryToPlaySpellCard(Spell spellCard)
    {

        if (allies.Count > 0)
        {
            if (allies[0].GetComponent<CharacterStats>() != null)
            {
                SpellEffectApplier.ApplySpell(spellCard, allies[0].GetComponent<CharacterStats>());
                handManager.cardsInAIHand.Remove(actualCart);
                discardManager.AddToDiscard(cardData);
                handManager.UpdateHandVisuals();
            }

        }

        handManager.cardsInAIHand.Remove(actualCart);
        discardManager.AddToDiscard(cardData);
        handManager.UpdateHandVisuals();
    }


}
