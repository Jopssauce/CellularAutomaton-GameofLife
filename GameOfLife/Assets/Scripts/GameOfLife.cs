using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        model.InitializeGame();
    }

    private void Start()
    {
        Camera.main.orthographicSize = sizeY / 2;
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
