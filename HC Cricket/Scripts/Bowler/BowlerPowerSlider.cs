using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BowlerPowerSlider : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private Slider powerSlider;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;

    [Header(" Events ")]
    public static Action<float> onPowerSliderStopped;

    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            Move();
    }

    public void StartMoving()
    {
        canMove = true;
    }

    public void StopMoving()
    {
        if (!canMove)
            return;

        //if we reach this point, then canMove is true
        canMove = false;
        onPowerSliderStopped?.Invoke(powerSlider.value);
    }

    private void Move()
    {
        powerSlider.value = (Mathf.Sin(Time.time * moveSpeed) + 1 ) / 2;
    }
}
