using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromRingToMenuButton : MonoBehaviour
{
    public GameObject[] fortuneRingGui;
    [SerializeField]
    private GUI gui;

    [SerializeField] private FortuneRing fr;
    public void fromRingToMenuButtonPressed()
    {
        gui.hideDarkBackGround();
        GameObject.Find("SettingsButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("ScoreCounter").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("Logo").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("TapToStart").GetComponent<Animator>().Play("InTapToStart");
        GameObject.Find("ShopButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayIn();
        //GameObject.Find("UpgradeButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("FortuneRingButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("HighScore").GetComponent<MoveAnimation>().PlayIn();
        gui.setScoreCounterText("");
        GameObject.Find("FortuneRing").GetComponent<MoveAnimation>().PlayIn();
    }
    
}
