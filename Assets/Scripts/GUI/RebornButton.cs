using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RebornButton : MonoBehaviour
{
    public Jelly jelly;
    [SerializeField]
    private GameField gameField;

    public void rebornButtonPressed(){
        GameObject.Find("DarkBackground").GetComponent<DarkBackgroundAnimation>().hide();
        GameObject.Find("RebornButton").GetComponent<MoveAnimation>().PlayIn();
        GameObject.Find("HighScore").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("MainMenuButton").GetComponent<MoveAnimation>().PlayIn();
        jelly.gui.foodProgressPanel.SetActive(true);
        gameField.fieldController.clearField();
        gameField.fieldController.generateMap();
        gameField.fieldController.animateGeneration();
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = true;
        GetComponent<MobAdsRewardedReborn>().jelly = jelly;
        GetComponent<MobAdsRewardedReborn>().showRewardedAd();
        //jelly.ContinueGame();
    }
}
