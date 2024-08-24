using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//using this library to be able to change text in score
using System;

public class LocalScoreManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private TextMeshProUGUI[] scoreTexts;

    [Header(" Settings ")]
    [SerializeField] private float firstRingRadius;
    [SerializeField] private float secondRingRadius;
    private int currentOver;

    [Header(" Events ")]
    public static Action<int> onScoreCalculated;

    // Start is called before the first frame update
    void Start()
    {
        ClearTexts();

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallMissed += BallMissedCallback;
        Ball.onBallFellInWater += BallHitGroundCallback;
    }

    private void OnDestroy()
    {
        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallMissed -= BallMissedCallback;
        Ball.onBallFellInWater -= BallHitGroundCallback;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void BallHitGroundCallback(Vector3 ballHitPosition)
    {
        //1. Calculating the score we need to add to the batsman
        float ballDistance = ballHitPosition.magnitude;

        int score = 2;

        if (ballDistance > firstRingRadius)
            score += 2;
        if (ballDistance > secondRingRadius)
            score += 2;

        //we know the value of the score at this point 
        //this is the amount the current batsman scored.

        onScoreCalculated?.Invoke(score);

        scoreTexts[currentOver].text = score.ToString();

        currentOver++;
    }

    private void BallMissedCallback()
    {
        scoreTexts[currentOver].text = "0";
        currentOver++;

        onScoreCalculated?.Invoke(0);
    }

    private void ClearTexts()
    {
        for (int i = 0; i < scoreTexts.Length; i++)
            scoreTexts[i].text = "";
    }
}
