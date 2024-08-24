using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
// there is a class of random in unity engine as well as in system so to make sure we use the one of unityengine we're specifying above, this way the script knows which randomto use.

public class AiBatsman : MonoBehaviour
{
    enum State { Moving, Hitting}

    [Header(" Elements ")]
    [SerializeField] private BowlerTarget bowlerTarget;
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider batCollider;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask ballMask;
    [SerializeField] private Vector2 minMaxHitVelocity;
    [SerializeField] private float maxHitDuration;

    private State state;
    private bool canDetectHits;
    private float hitTimer;

    [Header(" Events ")]
    public static Action<Transform> onBallHit;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Moving;
        PlayerBowler.onBallThrown += BallThrownCallback;

        BowlerManager.onNextOverSet += Restart;
    }

    private void OnDestroy()
    {
        PlayerBowler.onBallThrown -= BallThrownCallback;
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
            case State.Moving:
                Move();
                break;

            case State.Hitting:
                if (canDetectHits)
                    CheckForHits();
                break;
        }
    }

    private void Move()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = GetTargetX();

        targetPosition.x = Mathf.Clamp(targetPosition.x, -1.83f, 1.83f);

        // Calculating how far we are from the target X
        float difference = targetPosition.x - transform.position.x;


        if (difference == 0)
            //Play the idle animation
            animator.Play("Idle");
        else if (difference > 0)
            //play the left strafe animation
            animator.Play("Left");
        else
            //play right strafe animation
            animator.Play("Right");



        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime); 
    }

    private void BallThrownCallback(float ballFlightDuration)
    {
        state = State.Hitting;

        StartCoroutine(WaitandHitCoroutine());

        IEnumerator WaitandHitCoroutine()
        {
            float bestDelay = ballFlightDuration - .7f;

            float delay = Random.Range(bestDelay - .1f, bestDelay + .1f);

            yield return new WaitForSeconds(delay);

            animator.Play("Hit");
        }

    }

    public void StartDetectingHits()
    {
        canDetectHits = true;

        hitTimer = 0;
    }

    private void CheckForHits()
    {
        Vector3 center = batCollider.transform.TransformPoint(batCollider.center);
        Vector3 halfExtents = 1.5f * batCollider.size / 2;
        Quaternion rotation = batCollider.transform.rotation;

        Collider[] detectedBalls = Physics.OverlapBox(center, halfExtents, rotation, ballMask);

        for(int i=0; i<detectedBalls.Length; i++)
        {
            BallDetectedCallback(detectedBalls[i]);
            return;
        }

        hitTimer += Time.deltaTime;
    }

    private void BallDetectedCallback(Collider ballCollider)
    {
        canDetectHits = false;

        ShootBall(ballCollider.transform);
    }

    private void ShootBall(Transform ball)
    {
        //Compare the Hit timer with the Max Duration
        //if hitTimer = 0 then it means that we have hit the ball at the end of the animation so its the perfect time ,sohitTimer = 0 -> maxHitVelocity
        //if hitTimer > maxHitDuration -> we hit the ball at minimum velocity 'minVelocity' 

        float lerp = Mathf.Clamp01(hitTimer / maxHitDuration);
        float hitVelocity = Mathf.Lerp(minMaxHitVelocity.y, minMaxHitVelocity.x, lerp);

        Vector3 hitVelocityVector = (Vector3.back + Vector3.up + Vector3.right * Random.Range(-1f, 1f)) * hitVelocity;

        ball.GetComponent<Ball>().GetHitByBat(hitVelocityVector);

        //ball.GetComponent<Rigidbody>().velocity = 

        onBallHit?.Invoke(ball);
    }
    private float GetTargetX()
    {
        Vector3 bowlerShootPosition = new Vector3(-1, 0, -9.5f);
        Vector3 shootDirection = (bowlerTarget.transform.position - bowlerShootPosition).normalized;

        float shootAngle = Vector3.Angle(Vector3.right, shootDirection);

        float bc = transform.position.z - bowlerShootPosition.z;
        float ab = bc / Mathf.Sin(shootAngle * Mathf.Deg2Rad);

        Vector3 targetAiPosition = shootDirection * ab;

        return targetAiPosition.x - .5f;
    }

    private void Restart()
    {
        state = State.Moving;
    }
}
