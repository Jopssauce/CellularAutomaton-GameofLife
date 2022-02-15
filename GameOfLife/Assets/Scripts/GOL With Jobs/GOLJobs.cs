using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
/// <summary>
/// Result = I was able to squeeze out a bit more performance by surpsingly using less performant code. Not practical at all, this can be improved!
/// Test was done with 10 worker threads.
/// </summary>
public class GOLJobs : MonoBehaviour
{
    [SerializeField]
    GameOfLife _controller;
    public int sizeX { get; private set; }
    GameObject mousePointer;
    Texture2D texture2D;
    //public Cell[,] cells;
    public NativeArray<Vector2> size;
    public NativeArray<Cell.State> cellsStates;
    public NativeArray<Vector4> cellsColors;
    public NativeArray<Vector2> cellsPosition;
    public NativeArray<int> cellsNeighborCount;
    public NativeArray<int> cellsLiveNeigbors;
    public NativeArray<Color32> rawTexture2D;

    float timer = 1;
    int cells;
    private void Awake()
    {
        if (_controller == null)
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
            GetLiveNeighborsJob getLiveNeighborsJob = new GetLiveNeighborsJob()
            {
                cellsStates = cellsStates,
                cellsLiveNeigbors = cellsLiveNeigbors,
                cellsPosition = cellsPosition,
                size = size,
            };
            JobHandle getLiveNeighborsHandle = getLiveNeighborsJob.Schedule(cells, 1);
            getLiveNeighborsHandle.Complete();

            /// There is currently an issue where single cells arent dying or being updated at the texture 
            /// This only happens using Jobs and was fixed by adding an else statement to kill a cell at line:36 of ProcessGenerationsJob.cs

            rawTexture2D = texture2D.GetRawTextureData<Color32>();
            ProcessGenerationsJob processGenerationsJob = new ProcessGenerationsJob()
            {
                cellsStates = cellsStates,
                cellsLiveNeigbors = cellsLiveNeigbors,
                rawTexture2D = rawTexture2D
            };
            JobHandle processGenerationsHandle = processGenerationsJob.Schedule(cells, 1, getLiveNeighborsHandle);
            processGenerationsHandle.Complete();
            texture2D.LoadRawTextureData(rawTexture2D);

            //for (int i = 0; i < cells; i++)
            //{
            //    ProcessGeneration(i, cellsLiveNeigbors[i]);
            //}
            texture2D.Apply();
        }
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
        }

    }

    public void SetState(Cell.State state, int index)
    {
        cellsStates[index] = state;
        Vector2Int gridPos = new Vector2Int((int)cellsPosition[index].x, (int)cellsPosition[index].y);
        switch (cellsStates[index])
        {
            case Cell.State.Dead:
                texture2D.SetPixel(gridPos.x, gridPos.y, Color.black);
                break;
            case Cell.State.Alive:
                texture2D.SetPixel(gridPos.x, gridPos.y, _controller.cellColor);
                break;
            default:
                break;
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
    }

    public void InitializeGame()
    {
        InitGrid();

        cells = sizeX * _controller.sizeY;
        size =
            new NativeArray<Vector2>(1, Allocator.Persistent);
        size[0] = new Vector2(sizeX, _controller.sizeY);
        cellsStates =
            new NativeArray<Cell.State>(cells, Allocator.Persistent);
        cellsColors =
            new NativeArray<Vector4>(cells, Allocator.Persistent);
        cellsPosition =
            new NativeArray<Vector2>(cells, Allocator.Persistent);
        cellsNeighborCount =
            new NativeArray<int>(cells, Allocator.Persistent);
        cellsLiveNeigbors =
            new NativeArray<int>(cells, Allocator.Persistent);

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
                    texture2D.SetPixel(x, y, _controller.cellColor);
                    cellsColors[x * sizeX + y]
                        = new Vector4(_controller.cellColor.r, _controller.cellColor.g, _controller.cellColor.b, 1);
                }
                else
                {
                    state = Cell.State.Dead;
                    texture2D.SetPixel(x, y, Color.black);
                    cellsColors[x * sizeX + y] = new Vector4(Color.black.r, Color.black.g, Color.black.b, 1);
                }
                cellsPosition[x * sizeX + y] = new Vector2(x, y);
                cellsStates[x * sizeX + y] = state;
            }
        }

        texture2D.Apply();
    }

    //void ProcessInputPainting()
    //{
    //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    //    //World Coordinates
    //    int worldX = Mathf.FloorToInt(worldPos.x / sizeX * texture2D.width);
    //    int worldY = Mathf.FloorToInt(worldPos.y / _controller.sizeY * texture2D.height);

    //    //Grid Coordinates
    //    int gridX = worldX + sizeX / 2;
    //    int gridY = worldY + _controller.sizeY / 2;


    //    if (IsInside(gridX, gridY))
    //    {
    //        Cell currentHoveredCell = cells[gridX, gridY];

    //        //Centered World Coordinates
    //        if (_controller.fillCameraArea)
    //        {
    //            mousePointer.transform.position = new Vector3(worldX, worldY + 0.5f, 0);
    //        }
    //        else
    //        {
    //            mousePointer.transform.position = new Vector3(worldX + 0.5f, worldY + 0.5f, 0);
    //        }


    //        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
    //        {
    //            currentHoveredCell.SetState(Cell.State.Alive);
    //        }
    //        if (Input.GetMouseButtonDown(1) || Input.GetMouseButton(1))
    //        {
    //            currentHoveredCell.SetState(Cell.State.Dead);
    //        }
    //    }
    //}

    //public void ForceClear()
    //{
    //    for (int y = 0; y < cells.GetLength(1); y++)
    //    {
    //        for (int x = 0; x < sizeX; x++)
    //        {
    //            Cell currentCell = cells[x, y];
    //            currentCell.SetState(Cell.State.Dead);
    //        }
    //    }
    //}

    //public bool IsInside(int x, int y)
    //{
    //    if (x >= 0 && x < cells.GetLength(0) &&
    //        y >= 0 && y < cells.GetLength(1))
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    private void OnDestroy()
    {
        size.Dispose();
        cellsStates.Dispose();
        cellsColors.Dispose();
        cellsPosition.Dispose();
        cellsNeighborCount.Dispose();
        cellsLiveNeigbors.Dispose();
        if (rawTexture2D.IsCreated)
            rawTexture2D.Dispose();
    }
}
