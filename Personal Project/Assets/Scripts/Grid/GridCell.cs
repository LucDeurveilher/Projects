using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2 gridIndex;

    public bool cellFull = false;

    public GameObject objectInCell;

    private void Update()
    {
        if (objectInCell == null && cellFull) 
        {
            cellFull = false ;
        }
    }

}
