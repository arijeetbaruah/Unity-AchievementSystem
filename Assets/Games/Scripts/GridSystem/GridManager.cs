using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField, MinValue(1)]
    private int width;
    [SerializeField, MinValue(1)]
    private int height;
    [SerializeField, MinValue(1)]
    private float cellSize = 1;
    [SerializeField]
    private Vector2 offset;

    private Grid<GameObject> grid;
    GameObject gridHolder;

    private void Awake()
    {
        grid = new Grid<GameObject>(width, height, cellSize, offset, (x, y) =>
        {
            GameObject cell = new GameObject();
            cell.transform.SetParent(transform);

            return cell;
        });
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Grid<int> grid = new Grid<int>(width, height, cellSize, offset, (x, y) =>
        {
            return 0;
        });

        //Gizmos.color = Color.red;
        for(int x = 0; x < width - 1; x++)
        {
            for (int y = 0; y < height - 1; y++)
            {
                Vector3 startPos = grid.GetCell(x, y).GetPosition();
                Vector3 endPos = grid.GetCell(x + 1, y).GetPosition();
                Gizmos.DrawLine(startPos, endPos);

                startPos = grid.GetCell(x, y).GetPosition();
                endPos = grid.GetCell(x, y + 1).GetPosition();
                Gizmos.DrawLine(startPos, endPos);
            }
        }

        Vector3 startPos1 = grid.GetCell(0, height - 1).GetPosition();
        Vector3 endPos1 = grid.GetCell(width - 1, height - 1).GetPosition();
        Gizmos.DrawLine(startPos1, endPos1);

        startPos1 = grid.GetCell(width - 1, 0).GetPosition();
        endPos1 = grid.GetCell(width - 1, height - 1).GetPosition();
        Gizmos.DrawLine(startPos1, endPos1);
    }
#endif
}
