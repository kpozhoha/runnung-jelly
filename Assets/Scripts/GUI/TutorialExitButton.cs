using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExitButton : MonoBehaviour
{
    public GUI gui;
    
    public void tutorialExitButtonPressed()
    {
        gui.hideTutorial();
        gui.hideDarkBackGround();
    }
}
