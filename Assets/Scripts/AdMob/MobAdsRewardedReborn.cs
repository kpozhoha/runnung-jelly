using System;
using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using UnityEngine;

public class MobAdsRewardedReborn : MonoBehaviour
{

    public Jelly jelly;
    private RewardedAd _rewardedAd;
    
    #if UNITY_ANDROID
        private const string rewardedUnitId = "ca-app-pub-3940256099942544/8691691433";
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
            jelly.ContinueGame();
        }
        // _rewardedAd = new RewardedAd(rewardedUnitId);
        // AdRequest adRequest = new AdRequest.Builder().Build();
        // _rewardedAd.LoadAd(adRequest);
        // _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        jelly.ContinueGame();
    }
}