using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibroButton : MonoBehaviour
{
    public static bool vibroEnabled = true;


    public void vibroButtonPressed()
    {
        if (vibroEnabled)
        {
            vibroEnabled = false;
            GetComponent<Image>().color = Color.black;
        }
        else
        {
            vibroEnabled = true;
            GetComponent<Image>().color = Color.white;
        }
    }
}
