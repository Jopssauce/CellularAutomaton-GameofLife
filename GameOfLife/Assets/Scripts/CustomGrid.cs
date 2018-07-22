using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrid : MonoBehaviour
{
    public GameObject cube;
    public LayerMask m_unwalkableMask;
    public Vector3 m_worldSize;
    public float m_radius;
    public int randomFrequency = 10;
    public bool simulate = true;
    Vector3 bottomLeftCorner;
    public Cell[,] grid { get; private set; }
    public GameObject[,] cubes { get; private set; }

    //Node Size
    float m_nodeDiameter;
    //Grid Size
    public int m_sizeX { get; private set; }
    public int m_sizeZ { get; private set; }

    public int m_maxSize
    {
        get { return m_sizeX * m_sizeZ; }
    }

    void Start()
    {
        m_nodeDiameter = m_radius * 2;
        m_sizeX = Mathf.RoundToInt(m_worldSize.x / m_nodeDiameter);
        m_sizeZ = Mathf.RoundToInt(m_worldSize.z / m_nodeDiameter);
        CreateGrid();
    }

    void Update()
    {
        foreach (Cell node in grid)
        {
            cubes[node.m_gridX, node.m_gridZ].GetComponent<Renderer>().material.color = (node.isAlive) ? Color.white : Color.black;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            simulate = !simulate;
        }
    }


    void CreateGrid()
    {
        grid = new Cell[m_sizeX, m_sizeZ];
        cubes = new GameObject[m_sizeX, m_sizeZ];
        bottomLeftCorner = transform.position - Vector3.right * m_worldSize.x / 2 - Vector3.forward * m_worldSize.z / 2;
        for (int x = 0; x < m_sizeX; x++)
        {
            for (int z = 0; z < m_sizeZ; z++)
            {
                //Populates grid from bottom left corner
                Vector3 worldPoint = bottomLeftCorner + Vector3.right * (x * m_nodeDiameter + m_radius) + Vector3.forward * (z * m_nodeDiameter + m_radius);
                GameObject temp = Instantiate(cube, worldPoint, cube.transform.rotation);
                temp.transform.localScale = new Vector3(m_nodeDiameter - 0.1f, 0.1f, m_nodeDiameter - 0.1f);
                cubes[x, z] = temp;
                grid[x, z] = new Cell(false, worldPoint, x, z);
            }
        }
        //SetRandomAliveCells();
    }

    void SetRandomAliveCells()
    {
        for (int i = 0; i < m_sizeX * m_sizeZ; i++)
        {
            int randX = Random.Range(0, m_sizeX);
            int randZ = Random.Range(0, m_sizeZ);
            grid[randX, randZ].isAlive = true;
        }
    }

    public Cell GetNodeFromWorldPos(Vector3 pos)
    {
        float percentX = ((pos.x + m_worldSize.x / 2) / m_worldSize.x);
        float percentZ = ((pos.z + m_worldSize.z / 2) / m_worldSize.z);
        int x = Mathf.RoundToInt((m_sizeX - 1) * percentX);
        int z = Mathf.RoundToInt((m_sizeZ - 1) * percentZ);
        return grid[x, z];
    }

    public void ToggleNodeFromWorldPos(Vector3 pos)
    {
        Cell cell = GetNodeFromWorldPos(pos);
        cell.isAlive = !cell.isAlive;
    }

    public bool IsNodeInsideGrid(int x, int z)
    {
        if (x >= 0 && x < m_sizeX && z >= 0 && z < m_sizeZ)
            return true;
        else
            return false;
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.black;
        // Gizmos.DrawWireCube(transform.position, m_worldSize);
        // if (grid == null) return;
        // foreach (Cell node in grid)
        // {
        //     Gizmos.color = (node.isAlive) ? Color.white : Color.black;
        //     Gizmos.DrawCube(node.worldPos, Vector3.one * (m_nodeDiameter - 0.1f));
        // }


    }
}
