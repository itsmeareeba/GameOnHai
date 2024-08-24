using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private CanvasGroup transitionCG;
    [SerializeField] private TextMeshProUGUI transitionGameModeText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.onGameSet += GameSetCallback;
    }

    private void OnDestroy()
    {
        GameManager.onGameSet -= GameSetCallback;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameSetCallback()
    {
        if(GameManager.instance.isBowler())
        {
            transitionGameModeText.text = "BOWL";
        }
        else
        {
            transitionGameModeText.text = "BAT";
        }

        //show the transition panel
        //transitionCG.alpha = 1;
        LeanTween.alphaCanvas(transitionCG, 1, .5f);
        transitionCG.blocksRaycasts = true;
        transitionCG.interactable = true;
    }
}
