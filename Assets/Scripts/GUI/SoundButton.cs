using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    private float oldVolume;

    public void onSoundButtonPressed()
    {
        if (AudioListener.volume == 0)
        {
            GetComponent<Image>().color = Color.white;
            AudioListener.volume = oldVolume;
        }
        else
        {
            GetComponent<Image>().color = Color.black;
            oldVolume = AudioListener.volume;
            AudioListener.volume = 0;
        }
    }
}
