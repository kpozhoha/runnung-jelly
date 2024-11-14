using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelButton : MonoBehaviour
{
    public GameField gameField;
    public Jelly jelly;
    public GUI gui;
    public void nextLevelButtonPressed()
    {
        gui.hideDarkBackGround();
        gui.setScoreCounterText("0");
        GameObject.Find("NextLevelButton").GetComponent<MoveAnimation>().PlayIn();
        //GameObject.Find("HighScore").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("MainMenuButton").GetComponent<MoveAnimation>().PlayIn();
        gui.foodProgressPanel.SetActive(true);
        gameField.fieldController.clearField();
        gameField.fieldController.generateMap();
        gameField.fieldController.animateGeneration();
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = true;
        jelly.startGame();
    }
}
