using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOfLifeUI : MonoBehaviour
{
    public GameOfLife gameOfLife;
    public TextMeshProUGUI gridSize;

    private void Start()
    {
        gridSize.text = gameOfLife.sizeX + "x" + gameOfLife.sizeY;
    }
}
