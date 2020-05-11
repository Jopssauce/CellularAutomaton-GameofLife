using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOfLife : MonoBehaviour
{
    public int size;
    [Range(1, 50)]
    public float gameSpeed = 20;
    public GameObject tilePrefab;

    public Cell[,] cells;
    public bool blankCanvas;

    GameObject mousePointer;
    bool isPaused = true;
    float timer;

    Texture2D texture2D;

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
        Camera.main.orthographicSize = size / 2;
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
            //texture2D.SetPixels32(colors);
            texture2D.Apply();
            //The reason I split them into stages is because updating cells at the same with with getting live neighbors causes the wrong behavior
            //I suspect that this causes inconsistency leading to certain cells never dying or behaving unexpectedly
        }
        if (isPaused)
        {
            mousePointer.SetActive(true);
            ProcessInputPainting();
            texture2D.Apply();
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
        //Initialize Texture
        texture2D = new Texture2D(size, size);
        texture2D.filterMode = FilterMode.Point;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.mainTexture = texture2D;
        spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 1);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {

                //GameObject tile = Instantiate(tilePrefab, grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), tilePrefab.transform.rotation);

                if (blankCanvas)
                {
                    cells[x, y] = new Cell(Cell.State.Dead, new Vector3Int(x, y, 0), texture2D);
                }
                else
                {
                    int range = Random.Range(0, 100);
                    Cell.State state = Cell.State.Dead;
                    if (range < 50)
                    {
                        state = Cell.State.Alive;
                        texture2D.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        state = Cell.State.Dead;
                        texture2D.SetPixel(x, y, Color.black);
                    }
                    cells[x, y] = new Cell(state, new Vector3Int(x, y, 0), texture2D);
                    
                }

            }
        }
        texture2D.Apply();
    }

    void ProcessInputPainting()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //World Coordinates
        int worldX = Mathf.FloorToInt(worldPos.x / size * texture2D.width);
        int worldY = Mathf.FloorToInt(worldPos.y / size * texture2D.height);

        //Grid Coordinates
        int gridX = worldX + size / 2;
        int gridY = worldY + size / 2;


        if (IsInside(gridX, gridY))
        {
            Cell currentHoveredCell = cells[gridX, gridY];

            //Centered World Coordinates
            mousePointer.transform.position = new Vector3(worldX + 0.5f, worldY + 0.5f, 0);

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
        //Gizmos.color = Color.blue;
        //for (int x = -1; x < size + 1; x++)
        //{
        //    for (int y = -1; y < size + 1; y++)
        //    {
        //        if (y == size || x == size || y == -1 || x == -1)
        //        {
        //            Gizmos.DrawCube(grid.GetCellCenterWorld(new Vector3Int(x, y, 0)), new Vector3(1, 1, 1));
        //        }
        //    }
        //}
    }

}
