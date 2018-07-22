using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    CustomGrid grid;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start()
    {
        grid = GetComponent<CustomGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                //if(grid.IsNodeInsideGrid( Mathf.RoundToInt(hit.collider.gameObject.transform.position.x), Mathf.RoundToInt(hit.collider.gameObject.transform.position.z)))
                {
                    grid.ToggleNodeFromWorldPos(new Vector3(hit.transform.position.x, 0, hit.transform.position.z));
                    //hit.collider.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(ray);
    }
}
