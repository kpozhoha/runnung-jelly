using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButton : MonoBehaviour
{
    public GameField gameField;
    public GUI gui;
    public void mainMenuButtonPressed()
    {  
        gui.hideDarkBackGround();
        Time.timeScale = GameField.MIN_TIMESCALE;
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = true;
        //gui.setCoinCounterText("");
        gui.foodProgressPanel.SetActive(false);
        gui.setScoreCounterText("");
        gui.showMainMenuGUI();
        GameObject.Find("TapToStart").GetComponent<Animator>().Play("InTapToStart");
        GameObject.Find("MainMenuButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("RebornButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("PauseButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject nxtLblBtn = GameObject.Find("NextLevelButton");
        if (nxtLblBtn.GetComponent<Button>().GetComponent<RectTransform>().anchoredPosition.x > -10)
        {
            nxtLblBtn.GetComponent<MoveAnimation>().PlayIn();
        }
        //GameObject.Find("LevelCounterBackground").GetComponent<Image>().enabled = false;
        gameField.ResetField();
        Jelly jelly = GameObject.FindGameObjectWithTag("Player").GetComponent<Jelly>();
        if (jelly != null) {
            jelly.RendererEnabled(true);
            jelly.GetComponent<Rigidbody>().useGravity = true;
        }
        GameField.setTimeScale(GameField.MIN_TIMESCALE);
        Time.timeScale = GameField.getTimeScale();
    }
        
    
    
}
