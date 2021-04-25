using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Logic & Calculations for the Game of Life
[RequireComponent(typeof(GameOfLife))]
public class GameOfLifeModel : MonoBehaviour
{
    [SerializeField]
    GameOfLife _controller;
    public int sizeX { get; private set; }
    GameObject mousePointer;
    Texture2D texture2D;
    public Cell[,] cells;
    float timer = 1;

    private void Awake()
    {
        if(_controller == null)
            _controller = this.gameObject.GetComponent<GameOfLife>();


        //Initialize Mouse Pointer
        mousePointer = Instantiate(_controller.tilePrefab);
        mousePointer.GetComponent<SpriteRenderer>().color = Color.red;
        mousePointer.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    public void GameUpdate() 
    {
        timer -= Time.deltaTime * _controller.gameSpeed;
        if (!_controller.isPainting && timer <= 0)
        {
            timer = 1;
            mousePointer.SetActive(false);
            //First Stage: Get cell live Neighbors
            for (int y = 0; y < _controller.sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Cell currentCell = cells[x, y];
                    currentCell.GetLiveNeigbors();
                }
            }

            //Second Stage: Process Cell Generation
            for (int y = 0; y < _controller.sizeY; y++)
            {
                for (int x = 0; x < sizeX; x++)
                {
                    Cell currentCell = cells[x, y];
                    currentCell.ProcessGeneration();
                }
            }
            texture2D.Apply();
        }
        if (_controller.isPainting)
        {
            mousePointer.SetActive(true);
            ProcessInputPainting();
            texture2D.Apply();
        }
    }

    public void InitGrid()
    {
        //Set sizeX to fill up the camera orthographic area
        if (_controller.fillCameraArea)
        {
            sizeX = (_controller.sizeY * Screen.width) / Screen.height;
        }
        else
        {
            sizeX = _controller.sizeY;
        }

        cells = new Cell[sizeX, _controller.sizeY];
    }

    public void InitializeGame()
    {
        InitGrid();

        //Initialize Texture
        texture2D = new Texture2D(sizeX, _controller.sizeY);
        texture2D.filterMode = FilterMode.Point;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.mainTexture = texture2D;
        spriteRenderer.sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 1);

        Populate();

    }

    public void Populate()
    {
        //Setup Cells Grid and Paint Texture
        for (int y = 0; y < _controller.sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
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

        for (int y = 0; y < _controller.sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                cells[x, y].GetNeigbors(cells);
            }
        }
        texture2D.Apply();
    }

    void ProcessInputPainting()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //World Coordinates
        int worldX = Mathf.FloorToInt(worldPos.x / sizeX * texture2D.width);
        int worldY = Mathf.FloorToInt(worldPos.y / _controller.sizeY * texture2D.height);

        //Grid Coordinates
        int gridX = worldX + sizeX / 2;
        int gridY = worldY + _controller.sizeY / 2;


        if (IsInside(gridX, gridY))
        {
            Cell currentHoveredCell = cells[gridX, gridY];

            //Centered World Coordinates
            if (_controller.fillCameraArea)
            {
                mousePointer.transform.position = new Vector3(worldX, worldY + 0.5f, 0);
            }
            else
            {
                mousePointer.transform.position = new Vector3(worldX + 0.5f, worldY + 0.5f, 0);
            }


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

    public void ForceClear()
    {
        for (int y = 0; y < cells.GetLength(1); y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                Cell currentCell = cells[x, y];
                currentCell.SetState(Cell.State.Dead);
            }
        }
    }

    public bool IsInside(int x, int y)
    {
        if (x >= 0 && x < cells.GetLength(0) &&
            y >= 0 && y < cells.GetLength(1))
        {
            return true;
        }
        return false;
    }
}
