using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  //allows us to access some classes related to scene management
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public enum GameState { Menu, Batsman, Bowler, Win, Lose, Draw}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header(" Elements ")]
    [SerializeField] private GameObject settingsPanel;

    [Header(" Settings ")]
    private GameState gameState;
    private GameState firstGameState;

    [Header(" Events ")]
    public static Action onGameSet;
    public static Action<GameState> onGameStateChanged;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        settingsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonCallback()
    {
        int randomStateIndex = Random.Range(0, 2);

        if (randomStateIndex == 0)
        {
            firstGameState = GameState.Batsman;
        }
        else
        {
            firstGameState = GameState.Bowler;
        }

        gameState = firstGameState;

        onGameSet?.Invoke();

        Invoke("StartGame", 2);
    }

    public void StartGame()
    {
        if (firstGameState == GameState.Bowler)
        {
            StartBowlerMode();
        }
        else
        {
            StartBatsmanMode();
        }
    }

    public bool TryStartingNextGameMode()
    {
        if(gameState == firstGameState)
        {
            LeanTween.delayedCall(2, StartNextGameMode);
            return true;
        }
        else
        {
            //Trigger the Win/Draw/Lose
            Debug.LogWarning("Trigger Win / Lose / Draw Mode !");
            FinishGame();
            return false;
        }
    }

    private void StartNextGameMode()
    {
        //Trigger the first game mode
        if (firstGameState == GameState.Bowler)
        {
            StartBatsmanMode();
        }
        else
        {
            StartBowlerMode();
        }
    }

    private void StartBowlerMode()
    {
        gameState = GameState.Bowler;
        SceneManager.LoadScene("Bowler");
    }

    private void StartBatsmanMode()
    {
        gameState = GameState.Batsman;
        SceneManager.LoadScene("Batsman");
    }

    private void FinishGame()
    {
        if(ScoreManager.instance.IsPlayerWin())
        {
            //set the win state
            SetGameState(GameState.Win);
        }
        else if (ScoreManager.instance.IsPlayerLose())
        {
            //Set the lose state
            SetGameState(GameState.Lose);
        }
        else
        {
            //set the draw state
            SetGameState(GameState.Draw);
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
        onGameStateChanged?.Invoke(gameState);
    }

    public void NextButtonCallback()
    {
        SetGameState(GameState.Menu);
        SceneManager.LoadScene("Main");
    }

    public bool isBowler()
    {
        return gameState == GameState.Bowler;
    }

    public void ShowSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }
}
