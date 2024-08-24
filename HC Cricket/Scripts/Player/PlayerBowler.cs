using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerBowler : MonoBehaviour
{
    public enum State {Idle, Aiming, Running, Bowling }

    [Header(" Elements ")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject fakeBall;
    [SerializeField] private BallLauncher ballLauncher;
    [SerializeField] private GameObject ballTarget;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float runDuration;
    [SerializeField] private float flightDurationMultiplier;
    private float runTimer;
    private float bowlingSpeed;
    private Vector3 initialPosition;

    [Header(" Events ")]
    public static Action<float> onBallThrown;

    private State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;

        initialPosition = transform.position;

        BowlerManager.onNextOverSet += Restart;
    }

    private void OnDestroy()
    {
        BowlerManager.onNextOverSet -= Restart;
    }
    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch(state)
        {
            case State.Idle:
                break;

            case State.Aiming:
                break;

            case State.Running:
                Run();
                break;

            case State.Bowling:
                break;
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

        ballLauncher.LaunchBall(from,to,flightDuration);

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
