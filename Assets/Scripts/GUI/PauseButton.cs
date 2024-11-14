using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject inputManager;
    private float savedTimeScale;

    public void pauseButtonPressed()
    {
        StartCoroutine(playInPauseAnimations());
    }
    
    IEnumerator playInPauseAnimations()
    {
        inputManager.SetActive(false);
        pauseMenu.SetActive(true);
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        yield return null;
    }
    
    public IEnumerator playOutPauseAnimations()
    {
        inputManager.SetActive(true);
        Time.timeScale = savedTimeScale;
        pauseMenu.SetActive(false);
        yield return null;
    }
}
