using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class AiBowler : MonoBehaviour
{
    public enum State { Idle, Aiming, Running, Bowling }

    [Header(" Elements ")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fakeBall;
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private GameObject ballTarget;
    [SerializeField] private BowlerTarget bowlerTarget;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runDuration;
    [SerializeField] private float flightDurationMultiplier;
    private float runTimer;
    private float bowlingSpeed;
    private Vector3 initialPosition;
    private float aimingTimer;

    [Header(" Events ")]
    public static Action<float> onBallThrown;

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        initialPosition = transform.position;

        BatsmanManager.onAimingStarted += StartAiming;
        BatsmanManager.onNextOverSet += Restart;
    }

    private void OnDestroy()
    {
        BatsmanManager.onAimingStarted -= StartAiming;
        BatsmanManager.onNextOverSet -= Restart;
    }
    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Aiming:
                Aim();
                break;

            case State.Running:
                Run();
                break;

            case State.Bowling:
                break;
        }
    }

    private void StartAiming()
    {
        state = State.Aiming;
        aimingTimer = 0;
    }
    private void Aim()
    {
        float x = Mathf.PerlinNoise(0, Time.time + 36);  // is a Randomvalue generated by perlin noise
        float y = Mathf.PerlinNoise(0, Time.time * 2);

        Vector2 targetPosition = new Vector2(x, y);

        bowlerTarget.Move(targetPosition);

        aimingTimer += Time.deltaTime;

        if(aimingTimer>2)
        {
            StartRunning(80);
        }
    }

    public void StartRunning(float bowlingSpeed)
    {
        this.bowlingSpeed = bowlingSpeed;

        runTimer = 0;

        state = State.Running;
        animator.SetInteger("State", 1);
    }

    private void Run()
    {
        transform.position += Vector3.forward * Time.deltaTime * moveSpeed;

        runTimer += Time.deltaTime;

        if (runTimer >= runDuration)
            StartBowling();
    }

    private void StartBowling()
    {
        state = State.Bowling;
        animator.SetInteger("State", 2);
    }

    public void ThrowBall()
    {
        fakeBall.SetActive(false);

        Vector3 from = fakeBall.transform.position;
        Vector3 to = ballTarget.transform.position;

        //Calculating the duration of flight depending on the bowling speed (velocity).
        //velocity = distance / time

        float distance = Vector3.Distance(from, to);
        float velocity = bowlingSpeed / 3.6f;

        float flightDuration = flightDurationMultiplier * distance / velocity;

        ballLauncher.LaunchBall(from, to, flightDuration);

        onBallThrown?.Invoke(flightDuration);
    }

    private void Restart()
    {
        state = State.Idle;
        transform.position = initialPosition;

        animator.SetInteger("State", 0);
        animator.Play("Idle");

        fakeBall.SetActive(true);
    }
}