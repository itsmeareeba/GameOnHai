using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class BatsmanManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject drawPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private CanvasGroup transitionCG;
    [SerializeField] private TextMeshProUGUI transitionScoreText;

    [Header(" End Game Text Scores ")]
    [SerializeField] private TextMeshProUGUI[] endScoreTexts;

    [Header(" Settings ")]
    [SerializeField] private Vector2 minMaxBowlingSpeed;
    [SerializeField] private AnimationCurve bowlingSpeedCurve;

    [Header(" Events ")]
    public static Action onAimingStarted;
    public static Action onBowlingStarted;
    public static Action onNextOverSet;

    private int currentOver;

    IEnumerator Start()
    {
        yield return null; // wait for 1 frame
        winPanel.SetActive(false);
        drawPanel.SetActive(false);
        losePanel.SetActive(false);

        StartAiming();

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallMissed += BallMissedCallback;
        Ball.onBallHitStump += BallHitStumpCallback;
        Ball.onBallFellInWater += BallHitGroundCallback;

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void OnDestroy()
    {
        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallMissed -= BallMissedCallback;
        Ball.onBallHitStump -= BallHitStumpCallback;
        Ball.onBallFellInWater -= BallHitGroundCallback;

        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void StartAiming()
    {
        onAimingStarted?.Invoke();
    }

    public void StartBowling()
    {
        //Now creating an event to tell the bowler camera to enable the bowling Camera
        onBowlingStarted?.Invoke();
    }

    private void BallHitGroundCallback(Vector3 ballHitPosition)
    {
        currentOver++;

        if (currentOver >= 3)
        {
            //we should either switch to the other game mode
            //Or we should end the game / Compare scores

            if(GameManager.instance.TryStartingNextGameMode())
            {
                UpdateTransitionScore();
                ShowTransitionPanel();
            }
            else
            {
                //this means that we ended the game!
                //this is when the game manger returns false
                UpdateEndGameScoreTexts();
            }
            //Debug.Log("Set next game mode");
        }
        else
        {
            //Restart the game and go to the next over
            SetNextOver();
        }
    }

    private void BallMissedCallback()
    {
        BallHitGroundCallback(Vector3.zero);
    }

    private void BallHitStumpCallback()
    {
        currentOver = 2;
        BallHitGroundCallback(Vector3.zero);
    }

    private void SetNextOver()
    {
        StartCoroutine(WaitAndRestart());

        IEnumerator WaitAndRestart()
        {
            yield return new WaitForSeconds(2);

            onNextOverSet?.Invoke();
            StartAiming();
        }
    }

    private void ShowTransitionPanel()
    {
        LeanTween.alphaCanvas(transitionCG, 1, .5f);
        transitionCG.blocksRaycasts = true;
        transitionCG.interactable = true;
    }

    private void UpdateTransitionScore()
    {
        transitionScoreText.text = "<color #00aaff>"+ScoreManager.instance.GetPlayerScore() + 
            "</color> - <color #ff5500>" + ScoreManager.instance.GetAiScore() +"</color>";
    }

    private void UpdateEndGameScoreTexts()
    {
        for (int i = 0; i < endScoreTexts.Length; i++)
        {
            endScoreTexts[i].text = "<color #00aaff>" + ScoreManager.instance.GetPlayerScore() +
            "</color> - <color #ff5500>" + ScoreManager.instance.GetAiScore() + "</color>";
        }
    }

    private void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.Win:
                ShowWinPanel();
                break;

            case GameState.Lose:
                ShowLosePanel();
                break;

            case GameState.Draw:
                ShowDrawPanel();
                break;
        }
    }

    public void ShowWinPanel()
    {
        winPanel.SetActive(true);
    }
    public void ShowLosePanel()
    {
        losePanel.SetActive(true);
    }
    public void ShowDrawPanel()
    {
        drawPanel.SetActive(true);
    }

    public void NextButtonCallback()
    {
        GameManager.instance.NextButtonCallback();
    }
}
