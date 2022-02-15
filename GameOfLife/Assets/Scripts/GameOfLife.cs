using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Controller for the Game Of Life
public class GameOfLife : MonoBehaviour
{
    public bool useJobs;
    public GameOfLifeModel model;
    public GOLJobs jobsModel;
    public GameObject tilePrefab;
    //Vertical Size
    public int sizeY
    {
        get { return _sizeY; }
        set
        {
            if (value <= 2)
            {
                _sizeY = 2;
            }
            else
            {
                _sizeY = value;
            }
        }
    }
    [Header("Default Settings")]
    [SerializeField]
    int _sizeY = 100;

    public int minSpeed = 1;
    public int maxSpeed = 50;
    [Range(1, 50)]
    public float gameSpeed = 20;
    public bool fillCameraArea;
    public bool isPainting = false;
    public int liveCells;
    public int currentGeneration;
    public Color cellColor;

    public int sizeX { get; private set; }

    // C# Events are used here because they are more performant than Unity Events
    // These events are called multiple times
    public System.Action<Cell> onCellUpdate;
    public System.Action onGenerationUpdate;

    private void Awake()
    {
        if (!useJobs)
            model.InitializeGame();
        else
            jobsModel.InitializeGame();
    }

    private void Start()
    {
        Camera.main.orthographicSize = sizeY / 2;

        onCellUpdate += CellUpdateCallback;
        onGenerationUpdate += (() =>
        {
            liveCells = 0;
            currentGeneration++;
        });

        if (!useJobs)
        {
            sizeX = model.sizeX;
        }
        else
        {
            sizeX = jobsModel.sizeX;
        }
    }

    private void Update()
    {
        if (!useJobs)
            model.GameUpdate();
        else
            jobsModel.GameUpdate();
    }

    void CellUpdateCallback(Cell cell)
    {
        if (cell.state == Cell.State.Alive)
        {
            liveCells++;
        }
        cell.liveColor = cellColor;
    }

    public void RestartGame()
    {
        //model.ForceClear();
        if (!useJobs)
            model.InitializeGame();
        else
            jobsModel.InitializeGame();
        Camera.main.orthographicSize = sizeY / 2;
    }

}
