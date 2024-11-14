using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemBuyBuyButton : MonoBehaviour {
    [SerializeField] private Sprite emptyFrame;
    public Sprite EmptyFrame {
        get {
            return emptyFrame;
        }
    }
    [SerializeField] private Sprite chosenFrame;
    public Sprite ChosenFrame {
        get {
            return chosenFrame;
        }
    }
    [SerializeField] private Text price;
    [SerializeField] private Image frame;
    [SerializeField] private Sprite skin;
    
    public Jelly jelly;

    public void Start()
    {
        Vibration.Init();
        frame.sprite = emptyFrame;
        Button button = gameObject.GetComponent<Button>();
        int skinId = Int32.Parse(button.name);
        if (skinId < jelly.UnlockedSkins.Length && jelly.UnlockedSkins[skinId]) {
            ShopManager.ChangeSlotMode(gameObject, 
                jelly.ActiveSkin == skinId ? SlotMode.Active : SlotMode.Use);
            ShopManager.FindChildWithName(gameObject, "Text").GetComponent<Transform>().localPosition += new Vector3(25, 0, 0);
            ShopManager.FindChildWithName(gameObject, "ModelView").GetComponent<Image>().sprite = skin;
            price.text = "";
        }
    }
    
    public void pressed()
    {
        Button button = gameObject.GetComponent<Button>();
        string name = button.name;
        Text text = ShopManager.FindChildWithName(gameObject, "Text").GetComponent<Text>();
        if (text.text.ToLower().Equals("active")) {
            return;
        } else if (text.text.ToLower().Equals("use")) {
            ShopManager.MakeActive(gameObject);
            jelly.SetSkin(Int32.Parse(name));
        } else if (jelly.Coins >= Int32.Parse(price.text)) {
            ShopManager.MakeActive(gameObject);
            ShopManager.FindChildWithName(gameObject, "Text").GetComponent<Transform>().localPosition += new Vector3(25,0,0);
            ShopManager.FindChildWithName(gameObject, "ModelView").GetComponent<Image>().sprite = skin;
            jelly.spendCoins(Int32.Parse(price.text));
            price.text = "";
            jelly.addSkin(Int32.Parse(name));
        } else {
            if (VibroButton.vibroEnabled)
            {
                long[] pattern = { 0, 60, 80, 50 };
                Vibration.Vibrate(pattern, -1);
            }
            GameObject.Find("NotEnoughCoins").GetComponent<Animation>().Play("NotEnoughMoney");
        }
        SaveSystem.Save(jelly);
    }
}
