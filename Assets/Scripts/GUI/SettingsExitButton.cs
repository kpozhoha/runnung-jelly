using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsExitButton : MonoBehaviour
{
    public void settingsExitButtonPressed()
    {
        GameObject.Find("SettingsMenu").GetComponent<Animation>().Play("HideSettingsMenu2");
        GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayIn();
    }
    
}
