using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [Header(" Sounds ")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource bowlerShootSound;
    [SerializeField] private AudioSource ballHitGroundSound;
    [SerializeField] private AudioSource ballFellInWaterSound;
    [SerializeField] private AudioSource wicketSound;
    [SerializeField] private AudioSource batHitBallSound;

    // Start is called before the first frame update
    void Start()
    {
        PlayerBowler.onBallThrown += PlayBowlerSound;
        PlayerBatsman.onBallHit += PlayBatHitBallSound;

        AiBowler.onBallThrown += PlayBowlerSound;
        AiBatsman.onBallHit += PlayBatHitBallSound;

        Ball.onBallHitGround += PlayBallHitGroundSound;
        Ball.onBallFellInWater += PlayBallFellInWaterSound;
        Ball.onBallHitStump += PlayWicketSound;
    }

    private void OnDestroy()
    {
        PlayerBowler.onBallThrown -= PlayBowlerSound;
        PlayerBatsman.onBallHit -= PlayBatHitBallSound;

        AiBowler.onBallThrown -= PlayBowlerSound;
        AiBatsman.onBallHit -= PlayBatHitBallSound;

        Ball.onBallHitGround -= PlayBallHitGroundSound;
        Ball.onBallFellInWater -= PlayBallFellInWaterSound;
        Ball.onBallHitStump -= PlayWicketSound;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayBowlerSound(float nothinguseful)
    {
        bowlerShootSound.pitch = Random.Range(1.2f, 1.3f);
        bowlerShootSound.Play();
    }

    private void PlayBallHitGroundSound(Vector3 nothingReallyUseful)
    {
        ballHitGroundSound.pitch = Random.Range(.95f, 1.05f);
        ballHitGroundSound.Play();
    }

    private void PlayBallFellInWaterSound(Vector3 nothingReallyUseful)
    {
        ballFellInWaterSound.pitch = Random.Range(.95f, 1.05f);
        ballFellInWaterSound.Play();
    }

    private void PlayWicketSound()
    {
        wicketSound.pitch = Random.Range(.9f, 1f);
        wicketSound.Play();
    }

    private void PlayBatHitBallSound(Transform nothingIThinkReallyUseful)
    {
        batHitBallSound.pitch = Random.Range(.9f, 1f);
        batHitBallSound.Play();
    }

    public void DisableSounds()
    {
        bowlerShootSound.volume = 0;
        ballHitGroundSound.volume = 0;
        ballFellInWaterSound.volume = 0;
        wicketSound.volume = 0;
        batHitBallSound.volume = 0;
        buttonSound.volume = 0;
    }

    public void EnableSounds()
    {
        bowlerShootSound.volume = 1;
        ballHitGroundSound.volume = 1;
        ballFellInWaterSound.volume = 1;
        wicketSound.volume = 1;
        batHitBallSound.volume = 1;
        buttonSound.volume = 1;
    }
}
