using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBowlerAnimator : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private AiBowler aiBowler;

    private void ThrowBall()
    {
        aiBowler.ThrowBall();
    }
}
