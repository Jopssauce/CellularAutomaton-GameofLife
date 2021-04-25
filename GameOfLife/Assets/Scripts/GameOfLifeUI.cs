using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class GameOfLifeUI : MonoBehaviour
{
    public GameOfLife       gameOfLife;
    [Header("UI Elements")]
    public TextMeshProUGUI  gridSize;
    public Button           restart;
    public TMP_InputField   gridSizeInput;
    public Slider           gameSpeed;
    public TextMeshProUGUI  gameSpeedValue;
    public Toggle           fillCameraArea;
    public Toggle           paintMode;

    [Header("Events")]
    [SerializeField]
    UnityEvent<float> onGameSpeedValueChanged;
    [SerializeField]
    UnityEvent<bool> onFillAreaValueChanged;
    [SerializeField]
    UnityEvent<bool> onPaintValueChanged;
    [SerializeField]
    UnityEvent onRestartClicked;


    private void Start()
    {
        // Grid Size
        UpdateGridInput();

        // Game Speed
        gameSpeed.value = gameOfLife.gameSpeed;
        gameSpeedValue.text = gameOfLife.gameSpeed.ToString();
        gameSpeed.minValue = gameOfLife.minSpeed;
        gameSpeed.maxValue = gameOfLife.maxSpeed;
        gameSpeed.onValueChanged.AddListener(onGameSpeedValueChanged.Invoke);

        // Fill Area on restart
        fillCameraArea.isOn = gameOfLife.fillCameraArea;
        fillCameraArea.onValueChanged.AddListener(onFillAreaValueChanged.Invoke);

        // Toggle Paint Mode
        paintMode.isOn = gameOfLife.isPainting;
        paintMode.onValueChanged.AddListener(onPaintValueChanged.Invoke);

        // Restart
        restart.onClick.AddListener(onRestartClicked.Invoke);
    }

    public void UpdateGameSpeed(float value)
    {
        gameSpeedValue.text = value.ToString();
        gameOfLife.gameSpeed = value;
    }

    public void UpdateFillArea(bool value)
    {
        gameOfLife.fillCameraArea = value;
    }

    public void UpdatePaintMode(bool value)
    {
        gameOfLife.isPainting = value;
    }

    public void Restart()
    {
        gameOfLife.sizeY = int.Parse(gridSizeInput.text);
        gameOfLife.RestartGame();
        UpdateGridInput();
    }

    void UpdateGridInput()
    {
        gridSize.text = gameOfLife.model.sizeX + "x" + gameOfLife.sizeY;
        gridSizeInput.text = gameOfLife.sizeY.ToString();
    }
}
