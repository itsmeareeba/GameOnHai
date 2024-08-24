using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header(" Settings ")]
    private int playerScore;
    private int aiScore;

    private void Awake()
    {
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
        GameManager.onGameSet += ResetScores;
        LocalScoreManager.onScoreCalculated += ScoreCalculatedCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameSet -= ResetScores;
        LocalScoreManager.onScoreCalculated -= ScoreCalculatedCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScoreCalculatedCallback(int score)
    {
        if(GameManager.instance.isBowler())
        {
            aiScore += score;
        }
        else
        {
            playerScore += score;
        }

        Debug.Log("Player Score : " + playerScore);
        Debug.Log("Ai Score : " + aiScore);
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }

    public int GetAiScore()
    {
        return aiScore;
    }

    public bool IsPlayerWin()
    {
        return playerScore > aiScore;
    }

    public bool IsPlayerLose()
    {
        return playerScore < aiScore;
    }

    public void ResetScores()
    {
        playerScore = 0;
        aiScore = 0;
    }
}
