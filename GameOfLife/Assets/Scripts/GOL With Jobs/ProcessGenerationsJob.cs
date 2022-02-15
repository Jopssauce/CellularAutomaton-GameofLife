using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct ProcessGenerationsJob : IJobParallelFor
{
    public NativeArray<Cell.State> cellsStates;
    [ReadOnly]
    public NativeArray<int> cellsLiveNeigbors;

    public NativeArray<Color32> rawTexture2D;
    public void Execute(int index)
    {
        ProcessGeneration(index, cellsLiveNeigbors[index]);
    }

    public void ProcessGeneration(int index, int liveNeighbors)
    {
        if (cellsStates[index] == Cell.State.Alive)
        {
            if (liveNeighbors < 2 || liveNeighbors > 3)
            {
                SetState(Cell.State.Dead, index);
            }
        }
        else
        {
            if (liveNeighbors == 3)
            {
                SetState(Cell.State.Alive, index);
            }
        }

    }

    public void SetState(Cell.State state, int index)
    {
        cellsStates[index] = state;
        switch (cellsStates[index])
        {
            case Cell.State.Dead:
                rawTexture2D[index] = Color.black;
               // texture2D.SetPixel(gridPos.x, gridPos.y, Color.black);
                break;
            case Cell.State.Alive:
                rawTexture2D[index] = Color.white;
                //texture2D.SetPixel(gridPos.x, gridPos.y, _controller.cellColor);
                break;
            default:
                break;
        }
    }
}
