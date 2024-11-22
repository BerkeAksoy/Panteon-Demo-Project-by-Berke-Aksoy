using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Run,
        Paint
    }

    private static GameManager _instance;

    private static int coinCount, tryCount;
    private GameInfoUIManager gameInfoUIManager;
    private PaintManager paintManager;

    [SerializeField] private static GameState gameState;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game manager is null.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance == this) { return; }
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetGameState(GameState.Run);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    public void Restart()
    {
        gameInfoUIManager = GameInfoUIManager.Instance;

        gameInfoUIManager.AbortTween();
        Character.runners.Clear();

        coinCount = 0;
        tryCount++;

        gameInfoUIManager.UpdateCoinText(coinCount);
        gameInfoUIManager.UpdateTryText(tryCount);

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
        SetGameState(GameState.Run);
        CameraManager.Instance.SetCamForRun(); // Delete after event implementetion
    }

    public void AddCoin(int value)
    {
        coinCount += value;
        if(gameInfoUIManager)
            gameInfoUIManager.UpdateCoinText(coinCount);
    }

    public void IncreaseTryCount()
    {
        tryCount++;
        if(gameInfoUIManager)
            gameInfoUIManager.UpdateTryText(tryCount);
    }

    public GameState GetGameState()
    {
        return gameState;
    }

    public void SetGameState(GameState state)
    {
        gameInfoUIManager = GameInfoUIManager.Instance;
        paintManager = PaintManager.Instance;

        gameState = state;

        switch (state)
        {
            case GameState.Start:
                gameInfoUIManager.gameObject.SetActive(false);
                paintManager.gameObject.SetActive(false);
                break;
            case GameState.Run:
                gameInfoUIManager.gameObject.SetActive(true);
                paintManager.gameObject.SetActive(false);
                break;
            case GameState.Paint:
                gameInfoUIManager.gameObject.SetActive(false);
                paintManager.gameObject.SetActive(true);
                break;
        }
    }
}
