using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(GridCell))]
public class GridCellDisplay : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Color highlighColor = Color.cyan;
    public Color posColor = Color.green;
    public Color negColor = Color.red;

    private Color originalColor;
    public GameObject[] backGrounds;
    private bool setBackground = false;
    public GridCell gridCell;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridCell = GetComponent<GridCell>();
        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        if (!setBackground)
        {
            SetBackground();
        }
    }

    private void OnMouseEnter()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Game)
        {
            if (!GameManager.Instance.PlayingCard)
            {
                spriteRenderer.color = highlighColor;
            }
            else if (gridCell.cellFull || gridCell.gridIndex.x > 3)
            {
                spriteRenderer.color = negColor;
            }
            else
            {
                spriteRenderer.color = posColor;
            }
        }
    }

    private void OnMouseExit()
    {
        spriteRenderer.color = originalColor;
    }

    private void SetBackground()
    {
        if (gridCell.gridIndex.x % 2 != 0)
        {
            backGrounds[0].SetActive(true);
        }
        if (gridCell.gridIndex.y % 2 != 0)
        {
            backGrounds[1].SetActive(true);
        }
    }
}
