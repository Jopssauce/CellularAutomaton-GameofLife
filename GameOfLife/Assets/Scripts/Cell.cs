using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

	protected bool m_isAlive;
	protected Vector3 m_worldPos;

	public int m_gridX, m_gridZ;


	int m_heapIndex; 

	public bool isAlive
	{
		get {return m_isAlive;}
		set {m_isAlive = value;}
	}

	public Vector3 worldPos
	{
		get {return m_worldPos;}
		set {m_worldPos = value;}
	}

    public int HeapIndex
    {
        get
        {
			return m_heapIndex;
        }

        set
        {
			m_heapIndex = value;
        }
    }

    public Cell(bool walkable, Vector3 pos, int gridX, int gridZ)
	{
		m_isAlive = walkable;
		m_worldPos = pos;
		m_gridX = gridX;
		m_gridZ = gridZ;
	}


}
