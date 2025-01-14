using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Card;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 4;

    [SerializeField] float gridOffSetY = 0;

    public GameObject gridCellPrefab;
    public List<GameObject> gridObjects = new List<GameObject>();

    public GameObject[,] gridCells;

    void Start()
    {
        GameManager.Instance.ResetGame += ClearGrid;
        CreateGrid();
    }

    void CreateGrid()
    {
        gridCells = new GameObject[width, height];
        Vector2 centerOffset = new Vector2(width / 2.0f - 0.5f, height / 2.0f - 0.5f);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 gridPosition = new Vector2(x, y);
                Vector2 spawnPosition = gridPosition - centerOffset;
                spawnPosition.y += gridOffSetY;

                GameObject gridCell = Instantiate(gridCellPrefab, spawnPosition, Quaternion.identity);

                gridCell.transform.SetParent(transform);

                gridCell.GetComponent<GridCell>().gridIndex = gridPosition;

                gridCells[x, y] = gridCell;
            }
        }
    }

    public bool AddObjectToGrid(GameObject obj, Vector2 gridPosition)
    {
        if (gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height)
        {
            GridCell cell = gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();

            if (cell.cellFull)
            {
                return false;
            }
            else
            {
                GameObject newObj = Instantiate(obj, cell.GetComponent<Transform>().position, Quaternion.identity);
                newObj.transform.SetParent(transform);
                gridObjects.Add(newObj);
                cell.objectInCell = newObj;
                cell.cellFull = true;

               

                return true;
            }

        }
        else
        {
            Debug.Log("To Far");
            return false;
        }
    }

    public void RemoveObjectToGrid(GameObject obj)
    {
        if (gridObjects.Contains(obj))
        {
            // Trouver la cellule de la grille correspondant à cet objet
            foreach (GameObject cell in gridCells)
            {
                GridCell gridCellComponent = cell.GetComponent<GridCell>();
                if (gridCellComponent.objectInCell == obj)
                {
                    // Remettre à jour la cellule en libérant la référence à l'objet
                    gridCellComponent.objectInCell = null;
                    gridCellComponent.cellFull = false;
                    break;
                }
            }

            // Supprimer l'objet de la liste et le détruire
            gridObjects.Remove(obj);
            StartCoroutine(Utility.FadeOutGameObject(obj, 0, 0.9f));
            Destroy(obj, 1);

        }
    }

    public GridCell GetGridCell(Vector2 gridPosition)
    {
        return gridCells[(int)gridPosition.x, (int)gridPosition.y].GetComponent<GridCell>();
    }

    public GridCell GetGridCellByObjectIn(GameObject obj)
    {
        foreach (GameObject cell in gridCells)
        {
            if (cell.GetComponent<GridCell>().objectInCell == obj)
            {
                return cell.GetComponent<GridCell>();
            }
        }
        return null;
    }

    public void ClearGrid()
    {
        for (int i = gridObjects.Count - 1; i >= 0; i--)
        {
            RemoveObjectToGrid(gridObjects[i]);
        }
    }

}
