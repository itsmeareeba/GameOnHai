using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmanCamera : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private GameObject batsmanCamera;
    [SerializeField] private GameObject ballCamera;

    private void Awake()
    {
        BatsmanManager.onAimingStarted += EnableBatsmanCamera;

        PlayerBatsman.onBallHit += EnableBallCamera;

        Ball.onBallHitGround += BallHitGroundCallback;
        Ball.onBallFellInWater += BallHitGroundCallback;
    }

    private void OnDestroy()
    {
        BatsmanManager.onAimingStarted -= EnableBatsmanCamera;

        PlayerBatsman.onBallHit -= EnableBallCamera;

        Ball.onBallHitGround -= BallHitGroundCallback;
        Ball.onBallFellInWater -= BallHitGroundCallback;
    }

    private void EnableBatsmanCamera()
    {
        batsmanCamera.SetActive(true);
        ballCamera.SetActive(false);
    }
    private void EnableBallCamera(Transform ball)
    {
        ballCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = ball;
        ballCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = ball;

        ballCamera.SetActive(true);
        batsmanCamera.SetActive(false);
    }

    private void BallHitGroundCallback(Vector3 hitPosition)
    {
        ballCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
        ballCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = null;
    }
}
