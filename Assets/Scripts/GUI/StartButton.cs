using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public GameObject[] mainMenuElements;
    public Jelly jelly;
    public Text scoreText;
    [SerializeField] private GUI gui;

    public void startButtonPressed()
    {
        gui.coinCounterLabel.SetActive(false);
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = true;
        GameObject.Find("SettingsButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("TapToStart").GetComponent<Animator>().Play("OutTapToStart");
        foreach (GameObject element in mainMenuElements)
        {
            element.GetComponent<MoveAnimation>().PlayOut();
        }

        GameObject.Find("PauseButton").GetComponent<MoveAnimation>().finalPosition =
            GameObject.Find("SettingsButton").GetComponent<MoveAnimation>().startPosition;
        GameObject.Find("PauseButton").GetComponent<MoveAnimation>().PlayOut();
        
        setScoreCounterText("0");
        GetComponentInParent<GUI>().foodProgressPanel.SetActive(true);
        //GameObject.Find("LevelCounterBackground").GetComponent<Image>().enabled = true;
        jelly.startGame();
    }
    
    public void setScoreCounterText(string str)
    {
        scoreText.text = str;
    }
}
