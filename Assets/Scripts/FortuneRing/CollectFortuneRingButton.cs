using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFortuneRingButton : MonoBehaviour
{
    [SerializeField]
    private FortuneRing fr;

    public void collectFortuneRingPressed()
    {
        GameObject.Find("CoinCounter").GetComponent<AudioSource>().Play();
        fr.lotCollected();
        fr.jelly.achieveCoins(fr.lots[fr.whatWeWin]);
    }
}
