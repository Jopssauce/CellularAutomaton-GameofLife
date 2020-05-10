using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public Grid grid;
    public int size;
    [Range(1, 50)]
    public float gameSpeed = 20;
    public GameObject tilePrefab;

    public Cell[,] cells;
    public bool blankCanvas;

    GameObject mousePointer;
    bool isPaused = true;
    float timer;

    private void Awake()
    {
        cells = new Cell[size, size];
        //Initialize Mouse Pointer
        mousePointer = Instantiate(tilePrefab);
        mousePointer.GetComponent<SpriteRenderer>().color = Color.red;
        mousePointer.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    private void Start()
    {
        InitializeGame();
        timer = 1;
    }

    private void Update()
    {
        
        timer -= Time.deltaTime * gameSpeed;
        if (!isPaused && timer <= 0)
        {
            timer = 1;
            mousePointer.SetActive(false);
            //First Stage: Get cell live Neighbors
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell currentCell = cells[x, y];
                    currentCell.liveNeighbors = currentCell.GetCellLiveNeighbors(cells);

                }
            }

            //Second Stage: Process Cell Generation
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell currentCell = cells[x, y];
                    currentCell.ProcessGeneration();
                }
            }
            //The reason I split them into stages is because updating cells at the same with with getting live neighbors causes the wrong behavior
            //I suspect that this causes inconsistency leading to certain cells never dying or behaving unexpectedly
        }
        if (isPaused)
        {
            mousePointer.SetActive(true);
            ProcessInputPainting();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ForceClear();
        }

        CameraControls();
    }

    void InitializeGame()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {

                GameObject tile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), tilePrefab.transform.rotation);

                if (blankCanvas)
                {
                    cells[x, y] = new Cell(Cell.State.Dead, new Vector3Int(x, y, 0), tile.GetComponent<SpriteRenderer>());
                }
                else
                {
                    int range = Random.Range(0, 100);
                    Cell.State state = Cell.State.Dead;
                    if (range < 50)
                    {
                        state = Cell.State.Alive;
                    }
                    else
                    {
                        state = Cell.State.Dead;
                    }
                    cells[x, y] = new Cell(state, new Vector3Int(x, y, 0), tile.GetComponent<SpriteRenderer>());
                }

            }
        }
    }

    void ProcessInputPainting()
    {
        Vector3Int gridMousePos = grid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (IsInside(gridMousePos.x, gridMousePos.y))
        {
            Cell currentHoveredCell = cells[gridMousePos.x, gridMousePos.y];

            mousePointer.transform.position = grid.GetCellCenterWorld(currentHoveredCell.gridPos);

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                currentHoveredCell.SetState(Cell.State.Alive);
            }
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
            {
                currentHoveredCell.SetState(Cell.State.Dead);
            }
        }
    }

    void CameraControls()
    {
        float cameraSize = Camera.main.orthographicSize;
        if (Input.mouseScrollDelta.y != 0)
        {
            cameraSize += Input.mouseScrollDelta.y;
        }
        Camera.main.orthographicSize = Mathf.Clamp(cameraSize, 10, size / 2);
    }

    public void ForceClear()
    {
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell currentCell = cells[x, y];
                currentCell.SetState(Cell.State.Dead);
            }
        }
    }

    public bool IsInside(int x, int y)
    {
        if (x >= 0 && x < cells.GetLength(0) &&
            y >= 0 && y < cells.GetLength(0))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int x = -1; x < size + 1; x++)
        {
            for (int y = -1; y < size + 1; y++)
            {
                if (y == size || x == size || y == -1 || x == -1)
                {
                    Gizmos.DrawCube(grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), new Vector3(1, 1, 1));
                }
            }
        }
    }

}
