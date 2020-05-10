﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public State state { get; private set; }
    public Vector3Int gridPos;
    public SpriteRenderer spriteRenderer;

    public int liveNeighbors;
    public Cell(State state, Vector3Int gridPos, SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
        SetState(state);
        this.gridPos = gridPos;
    }

    public enum State
    {
        Dead,
        Alive
    }

    public void ProcessGeneration()
    {
        if (state == Cell.State.Alive)
        {
            if (liveNeighbors < 2 || liveNeighbors > 3)
            {
                SetState(Cell.State.Dead);
            }
        }
        else
        {
            if (liveNeighbors == 3)
            {
                SetState(Cell.State.Alive);
            }
        }

    }

    public void SetState(Cell.State state)
    {
        this.state = state;
        switch (this.state)
        {
            case State.Dead:
                spriteRenderer.color = Color.black;
                break;
            case State.Alive:
                spriteRenderer.color = Color.white;
                break;
            default:
                break;
        }
    }

    public int GetCellLiveNeighbors(Cell[,] cells)
    {
        int count = 0;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int xPos = gridPos.x + x;
                int yPos = gridPos.y + y;

                if (xPos >= 0 && xPos < cells.GetLength(0) &&
                    yPos >= 0 && yPos < cells.GetLength(0))
                {
                    if (cells[xPos, yPos].state == Cell.State.Alive)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

}
