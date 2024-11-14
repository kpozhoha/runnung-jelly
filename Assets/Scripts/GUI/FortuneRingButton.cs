using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneRingButton : MonoBehaviour
{
    [SerializeField]
    private GUI gui;

    [SerializeField] private FortuneRing fr;
    [SerializeField] public GameObject freePanel;

    
    public void fortuneRingPressed()
    {
        freePanel.SetActive(false);
        gui.showDarkBackGround();
        gui.setScoreCounterText("");
        GameObject.Find("TapToStart").GetComponent<Animator>().Play("OutTapToStart");
        GameObject.Find("ShopButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("Logo").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("SettingsButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayOut();
        //GameObject.Find("UpgradeButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("FortuneRingButton").GetComponent<MoveAnimation>().PlayOut();    
        GameObject.Find("RotatingRing").GetComponent<Transform>().rotation = new Quaternion(0, 0, 0, 0);
        GameObject.Find("HighScore").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("FortuneRing").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("ScoreCounter").GetComponent<MoveAnimation>().PlayOut();
        fr.setLots();
    }
}
