using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlayerBatsman : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider batCollider;

    [Header(" Settings ")]
    [SerializeField] private LayerMask ballMask;
    [SerializeField] private Vector2 minMaxHitVelocity;
    [SerializeField] private float maxHitDuration;

    [Header(" Movement Settings ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 MinMaxX;

    private Vector3 clickedPosition;
    private Vector3 clickedTargetPosition;

    private bool canDetectHits;
    private float hitTimer;

    [Header(" Events ")]
    public static Action<Transform> onBallHit;

    // Start is called before the first frame update
    void Start()
    {
        BowlerManager.onNextOverSet += Restart;
    }

    private void OnDestroy()
    {
        BowlerManager.onNextOverSet -= Restart;
    }

    // Update is called once per frame
    void Update()
    {
        ManageControl();

        if (canDetectHits)
            CheckForHits();
    }

    private void Move(float targetX)
    {
        Vector3 targetPosition = transform.position;
        targetPosition.x = targetX;

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

    public void StartDetectingHits()
    {
        canDetectHits = true;

        hitTimer = 0;
    }

    public void StopDetectingHits()
    {
        canDetectHits = false;
    }

    private void CheckForHits()
    {
        Vector3 center = batCollider.transform.TransformPoint(batCollider.center);
        Vector3 halfExtents = 1.5f * batCollider.size / 2;
        Quaternion rotation = batCollider.transform.rotation;

        Collider[] detectedBalls = Physics.OverlapBox(center, halfExtents, rotation, ballMask);

        for (int i = 0; i < detectedBalls.Length; i++)
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
    private void ManageControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedPosition = Input.mousePosition;
            clickedTargetPosition = transform.position;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 difference = Input.mousePosition - clickedPosition;

            difference.x /= Screen.width;

            float targetX = clickedTargetPosition.x - (difference.x * moveSpeed);

            Move(targetX);
        }
    }

    public void HitButtonCallback()
    {
        animator.Play("Player Hit");
    }

    private void Restart()
    {
        
    }
}
