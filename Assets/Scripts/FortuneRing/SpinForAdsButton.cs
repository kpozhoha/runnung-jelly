using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinForAdsButton : MonoBehaviour
{
    [SerializeField]
    private FortuneRing fr;
    
    public void spinForAdsButtonPressed()
    {
        fr.onRollPressed();   
    }
}
