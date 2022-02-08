using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct GetLiveNeighborsJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<Cell.State> cellsStates;
    [ReadOnly]
    public NativeArray<Vector2> cellsPosition;
    [ReadOnly]
    public NativeArray<Vector2> size;

    public NativeArray<int> cellsLiveNeigbors;
    public void Execute(int index)
    {
        cellsLiveNeigbors[index] = GetLiveNeigbors(index);
    }

    public int GetLiveNeigbors(int index)
    {
        Vector2 gridPos = cellsPosition[index];
        int count = 0;
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int xPos = (int)gridPos.x + x;
                int yPos = (int)gridPos.y + y;

                if (xPos >= 0 && xPos < size[0].x &&
                    yPos >= 0 && yPos < size[0].y)
                {
                    if (cellsStates[xPos * (int)size[0].x + yPos] == Cell.State.Alive)
                        count++;
                }

            }
        }
        return count;
    }
}