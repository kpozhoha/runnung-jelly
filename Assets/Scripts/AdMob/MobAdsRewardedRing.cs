using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class MobAdsRewardedRing : MonoBehaviour
{

    public FortuneRing fr;
    private RewardedAd _rewardedAd;
    
#if UNITY_ANDROID
    private const string rewardedUnitId = "ca-app-pub-3940256099942544/5224354917";
#endif

    private void OnEnable()
    {
        _rewardedAd = new RewardedAd(rewardedUnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        _rewardedAd.LoadAd(adRequest);
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    private void OnDisable()
    {
        _rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
    }

    public void showRewardedAd()
    {
        if (_rewardedAd.IsLoaded())
        {
            _rewardedAd.Show();
        }
        else
        {
            fr.lotCollected();
            fr.jelly.achieveCoins(2*fr.lots[fr.whatWeWin]);
        }
        // _rewardedAd = new RewardedAd(rewardedUnitId);
        // AdRequest adRequest = new AdRequest.Builder().Build();
        // _rewardedAd.LoadAd(adRequest);
        // _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        fr.lotCollected();
        fr.jelly.achieveCoins(2*fr.lots[fr.whatWeWin]);
    }
}