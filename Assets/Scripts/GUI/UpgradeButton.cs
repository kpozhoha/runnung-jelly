using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Jelly jelly;

    public void Start()
    {
        jelly = GameObject.Find("Jelly").GetComponent<Jelly>();
        GetComponentInChildren<Text>().text = jelly.upgradePrice.ToString();
    }
    public void upgradeButtonPressed()
    {
        if (jelly.Coins >= jelly.upgradePrice && !jelly.GetComponent<UpgradeAnimation>().IsRunning)
        {
            jelly.spendCoins(jelly.upgradePrice);
            jelly.upgrade();
            Vibration.Init();
            Vibration.VibratePop();
            GetComponentInChildren<Text>().text = jelly.upgradePrice.ToString();
        }
        
    }
}
