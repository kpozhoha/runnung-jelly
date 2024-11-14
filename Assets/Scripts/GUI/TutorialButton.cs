using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    public GUI gui;

    public void tutorialButtonPressed()
    {
        gui.showTutorial();
        gui.showDarkBackGround();
        GameObject.Find("SettingsMenu").GetComponent<Animation>().Play("HideSettingsMenu2");
    }
}
