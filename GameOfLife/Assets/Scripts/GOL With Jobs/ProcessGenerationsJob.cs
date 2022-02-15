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
            else
            {
                // This line is only necessary in Jobs as it causes cells to not die if not used
                SetState(Cell.State.Dead, index);
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
                break;
            case Cell.State.Alive:
                rawTexture2D[index] = Color.white;
                break;
            default:
                break;
        }
    }
}
