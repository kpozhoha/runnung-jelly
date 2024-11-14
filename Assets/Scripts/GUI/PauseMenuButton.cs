using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
{
    [SerializeField]
    private PauseButton pauseButton;

    public void PauseMenuButtonPressed() {
        StartCoroutine(pauseButton.playOutPauseAnimations());
        gameObject.SetActive(false);
    }
}
