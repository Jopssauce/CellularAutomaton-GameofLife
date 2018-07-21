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

    // Cells will always check their sorrounding neighbours 
    // 1. Cell will die with less than two neigbours
    // 2. Cell will live of there are 2 or three neigbours
    // 3. Cell will die if there are 3 or more cells
    // 4. Dead cells will live if there are 3 exactly three neigbours
    IEnumerator UpdateCells()
    {
        while (true)
        {
            List<Cell> neighbors = new List<Cell>();
            for (int x = 0; x < m_grid.m_sizeX; x++)
            {
                for (int z = 0; z < m_grid.m_sizeZ; z++)
                {
                    Cell cell = m_grid.grid[x, z];
                    neighbors = GetNeighbors(cell);

                    if (cell.isAlive && neighbors.Count < 2)
                        cell.isAlive = false;
                    if (cell.isAlive && neighbors.Count == 3 || neighbors.Count == 2)
                        cell.isAlive = true;
                    if (cell.isAlive && neighbors.Count > 1)
                        cell.isAlive = false;
                    if (cell.isAlive == false && neighbors.Count == 3)
                        cell.isAlive = true;
                    if (cell.m_gridX == m_grid.m_sizeX || cell.m_gridZ == m_grid.m_sizeZ)
                        cell.isAlive = false;
                    neighbors.Clear();
                }
                //yield return new WaitForSeconds(m_gameSpeed);
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
