using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour {
	CustomGrid grid;
	Ray ray;
	RaycastHit hit;
	// Use this for initialization
	void Start () {
		grid = GetComponent<CustomGrid>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit))
		{
			//if(grid.IsNodeInsideGrid( Mathf.RoundToInt(hit.collider.gameObject.transform.position.x), Mathf.RoundToInt(hit.collider.gameObject.transform.position.z)))
			{
				grid.ToggleNodeFromWorldPos(hit.transform.position);
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawRay(ray);
	}
}
