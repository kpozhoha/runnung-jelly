using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FortuneRing : MonoBehaviour
{

    public int whatWeWin;
    private float speed;
    private int numOfElements;

    private Transform ring;
    private GameObject rollingButton;
    private GameObject exitButton;

    [SerializeField]
    public int K;
    
    [SerializeField]
    private Text[] lotsText;

    public Jelly jelly;
    
    public int[] lots = {100, 200, 100, 200, 300, 100, 200, 500};
    private GameObject collectButton;
    private GameObject doubleWinButton;
    private GameObject spinForAdsButton;
    private Text timerText;
    private ParticleSystem particleSystem;
    private GameObject freePanel;

    public void Start()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        jelly = GameObject.Find("Jelly").GetComponent<Jelly>();
        collectButton = GameObject.Find("CollectFortuneRing");
        doubleWinButton = GameObject.Find("DoubleWin");
        spinForAdsButton = GameObject.Find("SpinForAdsButton");
        timerText = spinForAdsButton.GetComponentInChildren<Text>();
        spinForAdsButton.SetActive(false);
        collectButton.SetActive(false);
        doubleWinButton.SetActive(false);
        rollingButton = GameObject.Find("RollFortuneRingButton");
        exitButton = GameObject.Find("FromRingToMenuButton");
        numOfElements = lots.Length;
        ring = GameObject.Find("RotatingRing").GetComponent<Transform>();
        freePanel = GameObject.Find("FortuneRingButton").GetComponent<FortuneRingButton>().freePanel;
        if (!freeSpinAvailable())
        {
            freePanel.SetActive(false);
            StartCoroutine(freeSpinTimer());
        }
        else
        {
            freePanel.GetComponent<FreeSpinLabelAnimator>().play();
        }
        //K = jelly.Level / 10 + 1;
    }

    public void onRollPressed()
    {
        rollingButton.SetActive(false);
        exitButton.SetActive(false);
        if (spinForAdsButton.activeSelf)
        {
            spinForAdsButton.SetActive(false);
        }
        freePanel.SetActive(false);
        StartCoroutine(turnRing());
    }


    private IEnumerator turnRing()
    {
        speed = Random.Range(0.15f, 0.2f);
        float timeInterval = 0.01f;
        while (speed > 0.0001f) {
            ring.Rotate(0, 0, speed / timeInterval);
            speed -= 0.001f;
            yield return new WaitForSeconds(timeInterval);  
        }
        ring.Rotate(0, 0, speed / timeInterval);
        double p = 360 / numOfElements;
        double rotationAngle = ring.eulerAngles.z;
        if (p * 7.5f <= rotationAngle || rotationAngle <= p * 0.5f) {
            whatWeWin = 0;
        } else {
            for (int i = 1; i < numOfElements; i++) {
                if (p * (i - 0.5f) <= rotationAngle && rotationAngle <= p * (i + 0.5f)) {
                    whatWeWin = i;
                    break;
                }
            }
        }
        collectButton.SetActive(true);
        // PA RA SHA
        // Тут должен настраиваться желтый текст для lots[whatWeWin]

        // private Text winValue;
        // winValue.text = lots[whatWeWin].ToString();
        //winValue.color = new Color(0, 0, 0, 1);
   

       collectButton.GetComponentInChildren<Text>().text = "COLLECT\t" + lots[whatWeWin];
       doubleWinButton.SetActive(true);
    }


    public void lotCollected()
    {
        rollingButton.SetActive(true);
        exitButton.SetActive(true);
        collectButton.SetActive(false);
        doubleWinButton.SetActive(false);
        particleSystem.Play();
        setLots();
    }

    public void setLots()
    {
        string text = GameObject.Find("ShopButton").GetComponentInChildren<Text>().text;
        int price;
        if (text.Equals("")) {
            return;
        } else {
            price = int.Parse(text);
        }
        lots[7] = price / 50;
        lots[2] = price / 20;
        lots[4] = price / 20;
        lots[1] = price / 50;
        lots[6] = price / 10;
        lots[5] = price / 20;
        lots[3] = price / 10;
        lots[0] = price / 5;
        if (price / 5 >= 100000) {
            for (int i = 0; i < lots.Length; ++i) {
                lotsText[i].fontSize = 28;   
            }
        }
        for (int i = 0; i < lots.Length; i++)
        {
            lots[i] *= K;
            lotsText[i].text = lots[i].ToString().ToUpper();
        }
        Debug.Log(freeSpinAvailable());
        if (freeSpinAvailable())
        {
            rollingButton.GetComponentInChildren<Text>().text = "FREE SPIN";
            freePanel.SetActive(true);
            freePanel.GetComponent<FreeSpinLabelAnimator>().play();
        }
        else
        {
            spinForAdsButton.SetActive(true);
            rollingButton.SetActive(false);
            StartCoroutine(freeSpinTimer());
        }
    }

    public IEnumerator freeSpinTimer()
    {
        DateTime fourHours = jelly.freeSpinLastTime.AddHours(4);
        while (!freeSpinAvailable())
        {
            timerText.text = StripMilliseconds(fourHours.Subtract(DateTime.Now)).ToString("t");
            yield return new WaitForSeconds(1);
        }
        rollingButton.GetComponentInChildren<Text>().text = "FREE SPIN";
        spinForAdsButton.SetActive(false);
        rollingButton.SetActive(true);
        freePanel.SetActive(true);
        freePanel.GetComponent<FreeSpinLabelAnimator>().play();
    }
    
    
    public bool freeSpinAvailable()
    {
        return jelly.freeSpinLastTime.AddHours(4) <= DateTime.Now;
    }
    
    private TimeSpan StripMilliseconds(TimeSpan time)
    {
        return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
    }
}
