using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private SoundsManager soundsManager;
    [SerializeField] private Sprite optionsOnSprite;
    [SerializeField] private Sprite optionsOffSprite;
    [SerializeField] private Image soundsButtonImage;

    [Header(" Settings ")]
    private bool soundsState = true;

    private void Awake()
    {
        soundsState = PlayerPrefs.GetInt(" sounds ", 1) == 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Setup()
    {
        if (soundsState)
            EnableSounds();
        else
            DisableSounds();
    }

    public void ChangeSoundsState()
    {
        if (soundsState)
            DisableSounds();
        else
            EnableSounds();

        soundsState = !soundsState;

        //Save the value of SoundsState
        PlayerPrefs.SetInt(" sounds ", soundsState? 1: 0);
    }

    private void DisableSounds()
    {
        //tell the sounds manager to set the volume of all sounds to zero
        soundsManager.DisableSounds();

        //change the image of sound button
        soundsButtonImage.sprite = optionsOffSprite;
    }

    private void EnableSounds()
    {
        soundsManager.EnableSounds();
        //change the image of sound button
        soundsButtonImage.sprite = optionsOnSprite;
    }
}
