using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButton : MonoBehaviour
{
    public static Vector3 pos;
    
    public void Start()
    {
        pos = GameObject.Find("SettingsMenu").GetComponent<Transform>().position;
    }
    public void settingButtonPressed()
    {
        
        GameObject.Find("SettingsMenu").GetComponent<Animation>().Play("ShowSettingsMenu");
        GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayOut();
        
    }
}
