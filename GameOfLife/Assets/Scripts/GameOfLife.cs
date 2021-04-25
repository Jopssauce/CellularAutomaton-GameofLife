using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Controller for the Game Of Life
public class GameOfLife : MonoBehaviour
{
    public GameOfLifeModel model;
    public GameObject tilePrefab;
    //Vertical Size
    public int sizeY { 
        get { return _sizeY; } 
        set 
        {
            if(value <= 2)
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

    // C# Events are used here because they are more performant than Unity Events
    // These events are called multiple times
    public System.Action<Cell> onCellUpdate;
    public System.Action onGenerationUpdate;

    private void Awake()
    {
        model.InitializeGame();
    }

    private void Start()
    {
        Camera.main.orthographicSize = sizeY / 2;

        onCellUpdate += ((cell) =>
        {
            if (cell.state == Cell.State.Alive)
                liveCells++;
        });
        onGenerationUpdate += (() =>
        {
            liveCells = 0;
            currentGeneration++;
        });
    }

    private void Update()
    {
        model.GameUpdate();
    }

    public void RestartGame()
    {
        model.ForceClear();
        model.InitializeGame();
        Camera.main.orthographicSize = sizeY / 2;
    }

}
