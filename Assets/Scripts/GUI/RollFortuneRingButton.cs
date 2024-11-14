using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollFortuneRingButton : MonoBehaviour
{
    public FortuneRing fr;

    public void RollFortuneRingButtonPressed()
    {
        Jelly jelly = GameObject.Find("Jelly").GetComponent<Jelly>();
        if (fr.freeSpinAvailable())
        {
            fr.onRollPressed();
            jelly.freeSpinLastTime = DateTime.Now;
        }
    }

    
}
