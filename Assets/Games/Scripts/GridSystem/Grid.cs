using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid<T>
{
    private int width;
    private int height;
    private float cellSize;

    private Vector3 offset;

    private GridCell<T>[,] cells;

    public Grid(int width, int height, float cellSize, Vector3 offset, Func<int, int, T> initiator)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.offset = offset;

        cells = new GridCell<T> [ width, height];
        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                cells[x, y] = new GridCell<T>
                {
                    x = x,
                    y = y,
                    cellSize = cellSize,
                    offset = offset,
                    value = initiator(x, y)
                };
            }
        }
    }

    public void SetValue(int x, int y, T value)
    {
        cells[x, y].value = value;
    }

    public GridCell<T> GetCell(int x, int y)
    {
        return cells[x, y];
    }
}

public struct GridCell<T>
{
    public int x;
    public int y;
    public float cellSize;
    public Vector3 offset;

    public T value;

    public Vector3 GetPosition()
    {
        return new Vector3(x, 0, y) * cellSize + offset;
    }
}
