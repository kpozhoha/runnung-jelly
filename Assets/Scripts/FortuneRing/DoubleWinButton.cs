using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleWinButton : MonoBehaviour
{
    [SerializeField]
    private FortuneRing fr;

    public void doubleWinPressed()
    {
        GetComponent<MobAdsRewardedRing>().fr = fr;
        GetComponent<MobAdsRewardedRing>().showRewardedAd();
        //fr.lotCollected();
        //fr.jelly.achieveCoins(2*fr.lots[fr.whatWeWin]);
    }
}
