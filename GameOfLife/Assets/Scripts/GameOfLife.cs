using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public float m_gameSpeed = 1;
    public CustomGrid m_grid;
    IEnumerator updateCells;

    void Start()
    {
        updateCells = UpdateCells();
        StartCoroutine(updateCells);
    }

    // For a space that is 'populated':
    // Each cell with one or no neighbors dies, as if by solitude.
    // Each cell with four or more neighbors dies, as if by overpopulation.
    // Each cell with two or three neighbors survives.

    // For a space that is 'empty' or 'unpopulated'
    // Each cell with three neighbors becomes populated.
    IEnumerator UpdateCells()
    {
        while (true)
        {
            if (m_grid.simulate)
            {
                for (int z = 0; z < m_grid.m_sizeZ; z++)
                {
                    for (int x = 0; x < m_grid.m_sizeX; x++)
                    {
                        List<Cell> neighbors = new List<Cell>();
                        Cell cell = m_grid.grid[x, z];
                        neighbors = GetNeighbors(cell);

                        if(neighbors.Count < 2 || neighbors.Count > 3) cell.isAlive = false;
                        //if(neighbors.Count == 2 || neighbors.Count == 3) cell.isAlive = true;
                        if(!cell.isAlive && neighbors.Count == 3) cell.isAlive = true;
                        neighbors.Clear();
                    }
                }
            }

            yield return new WaitForSeconds(m_gameSpeed);
        }

    }

    public List<Cell> GetNeighbors(Cell current)
    {
        List<Cell> Neighbors = new List<Cell>();
        //Checks around current node
        for (int x = -1; x <= 1; x++)
        {
            for (int z = -1; z <= 1; z++)
            {
                if (x == 0 && z == 0)
                    continue;
                int xPos = current.m_gridX + x;
                int zPos = current.m_gridZ + z;
                if (m_grid.IsNodeInsideGrid(xPos, zPos))
                {
                    Cell cell = m_grid.grid[xPos, zPos];
                    if (cell.isAlive) Neighbors.Add(cell);
                }

                //Debug.Log( "Current Neigbor " +  currentNeigbor.m_gridX + " " + currentNeigbor.m_gridZ);
            }
        }
        return Neighbors;
    }


}
