using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball : MonoBehaviour
{
    [Header(" Settings ")]
    private bool hasHitGround;
    private bool hasBeenHitByBat;

    [Header(" Events ")]
    public static Action<Vector3> onBallHitGround;
    public static Action onBallMissed;
    public static Action onBallHitStump;
    public static Action<Vector3> onBallFellInWater;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Detect if the ball touches the field.
        if (collision.collider.tag == "Field")
            FieldCollidedCallback();
        if (collision.collider.tag == "Stump")
            StumpCollidedCallback();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Water")
            WaterTriggeredCallback();
        else if (collider.tag == "Miss")
            MissTriggeredCallback();

    }

    private void FieldCollidedCallback()
    {
        if (!hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround = true;

        onBallHitGround?.Invoke(transform.position);
    }

    private void WaterTriggeredCallback()
    {
        if (!hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround = true;

        onBallFellInWater?.Invoke(transform.position);
        //onBallHitGround?.Invoke(transform.position);
    }

    private void MissTriggeredCallback()
    {
        if (hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround = true;

        onBallMissed?.Invoke();
    }

    private void StumpCollidedCallback()
    {
        if (hasBeenHitByBat)
            return;

        if (hasHitGround)
            return;

        hasHitGround = true;

        onBallHitStump?.Invoke();
    }

    public void GetHitByBat(Vector3 velocity)
    {
        hasBeenHitByBat = true;
        GetComponent<Rigidbody>().velocity = velocity;
    }
}
