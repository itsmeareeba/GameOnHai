using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBatsmanAnimator : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private AiBatsman aiBatsman;
  public void StartDetectingHits()
    {
        aiBatsman.StartDetectingHits();
    }
}
