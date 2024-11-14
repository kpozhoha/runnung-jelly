using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    // [SerializeField]
    // private GUI gui;
    // private Vector3 cameraOldPositon;
    // public Vector3 offset = new Vector3(0,0,0);

    // public Vector3 getCameraOldPosition()
    // {
    //     return cameraOldPositon;
    // }
    [SerializeField]
    private GameObject[] pillars;
    [SerializeField]
    private int[] skinPrices;

    public int[] SkinPrices {
        get {
            return skinPrices;
        }
    }

    private AudioSource audioSource;

    private void Start() {
        if (pillars.Length != jelly.skins.Length - 1) {
            throw new System.Exception("amount of pillars must equal the amount of skins");
        }
        for (int i = 0; i < pillars.Length; ++i) {
            if (i >= jelly.ActiveSkin) {
                pillars[i].GetComponent<MoveAnimation>().PlayOutInstant();
            }
        }
        if (jelly.ActiveSkin >= pillars.Length) {
            GetComponentInChildren<Text>().text = "";
            GetComponentInParent<Button>().enabled = false;
            GetComponent<Image>().color = new Color32(102, 102, 102, 255);
        } else {
        GetComponentInChildren<Text>().text = skinPrices[jelly.ActiveSkin + 1].ToString();
        }
        audioSource = GetComponent<AudioSource>();
    }

    public Jelly jelly;
    public void shopButtonPressed()
    {
        if (jelly.Coins >= skinPrices[jelly.ActiveSkin + 1])
        {
            audioSource.Play();
            jelly.GetComponent<UpgradeAnimation>().Animate();
            pillars[jelly.ActiveSkin].GetComponent<MoveAnimation>().PlayIn();
            jelly.ActiveSkin += 1;
            Vibration.Init();
            Vibration.VibratePop();
            jelly.spendCoins(skinPrices[jelly.ActiveSkin]);
            jelly.addSkin(jelly.ActiveSkin);
            SaveSystem.Save(jelly);
            if (jelly.ActiveSkin == jelly.skins.Length - 1)
            {
                GetComponentInChildren<Text>().text = "";
                GetComponentInParent<Button>().enabled = false;
                GetComponent<Image>().color = new Color32(102, 102, 102, 255);
            } else {
                GetComponentInChildren<Text>().text = skinPrices[jelly.ActiveSkin + 1].ToString();
            }
        }
    }
    
    // IEnumerator zoomCamera()
    // {
    //     Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    //     cameraOldPositon = camera.transform.position;
    //     float time = 0.1f;
    //     while ((camera.transform.position - jelly.transform.position - new Vector3(0, 8, -15)).magnitude >= 1f)
    //     {
    //         time += 0.01f;
    //         camera.transform.position = Vector3.Lerp(camera.transform.position, jelly.transform.position + new Vector3(0,8,-15), time);
    //         yield return new WaitForSeconds(0.01f);
    //     }
    // }  
    
 }

// jelly.GetComponent<FoodAnimation>().enabled = true;
// jelly.GetComponent<Rigidbody>().useGravity = false;
// jelly.GetComponent<Rigidbody>().isKinematic = true;
// GameObject.Find("Shop").GetComponent<MoveAnimation>().PlayOut();
// gui.hideMainMenuGUI();
// GameObject.Find("TapToStart").GetComponent<Animator>().Play("OutTapToStart");
// StartCoroutine("zoomCamera");
// gui.setScoreCounterText("");
// GameObject.Find("ScoreCounter").GetComponent<MoveAnimation>().PlayOut();