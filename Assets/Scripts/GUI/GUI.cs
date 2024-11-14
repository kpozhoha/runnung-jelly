using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public GameObject swipeDetector;
    public Text scoreText;
    public GameObject foodProgressPanel;
    public Text levelText;
    public Text nextLevelText;
    public Text coinCounter;
    public Text coinsGetAnimator;
    public Text highScoreText;
    public GameObject[] mainMenuElements;
    public DarkBackgroundAnimation darkBackground;
    public GameObject tutorial;
    public Image newHighScoreFire;
    [SerializeField]
    public GameObject coinCounterLabel;
    [SerializeField]
    private Jelly jelly;

    [SerializeField] private GameObject platform;

    private bool newHighScoreIsAnimating = false;
    [SerializeField]
    private ParticleSystem levelPassedParticles;


    public void Start()
    {
        tutorial.SetActive(false);
        Jelly jelly = GameObject.FindGameObjectWithTag("Player").GetComponent<Jelly>();
        if (jelly.firstVisit) {
            showTutorial();
            showDarkBackGround();
            GameObject.Find("SettingsMenu").GetComponent<Animation>().Play("HideSettingsMenu2");
            jelly.firstVisit = false;
            SaveSystem.Save(jelly);
        }
    }
    public void levelPassed()
    {
        GameObject.Find("NextLevelButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("MainMenuButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ScoreCounter").GetComponent<Text>().text = "Level passed!";
        levelPassedParticles.Play();
    }
    public void die()
    {
        GameObject.Find("RebornButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("MainMenuButton").GetComponent<MoveAnimation>().PlayOut();
        GameObject.Find("PauseButton").GetComponent<Button>().interactable = false;
    }

    public void setCoinsGetAnimator(String str)
    {
        coinsGetAnimator.text = str;
    }
    
    public void setCoinCounterText(string str)
    {
        coinCounter.text = str;
    }
    
    public void setScoreCounterText(string str)
    {
        scoreText.text = str;
    }

    public void SetFoodProgressBar(float progress) {
        foodProgressPanel.GetComponentInChildren<Slider>().value = progress;
    }

    public void SetLevelCounterText(int lvl) {
        levelText.text = lvl.ToString();
        nextLevelText.text = (lvl + 1).ToString();
    }

    public void hideMainMenuGUI()
    {
        foreach (GameObject element in mainMenuElements)
        {
            if (element.name == "TapToStart")
            {
                element.GetComponent<Animator>().Play("OutTapToStart");
            }
            element.GetComponent<MoveAnimation>().PlayOut();
        }
    }

    public void showMainMenuGUI()
    {
        foreach (GameObject element in mainMenuElements)
        {
            if (element.name == "TapToStart")
            {
                element.GetComponent<Animator>().Play("InTapToStart");
            }
            element.GetComponent<MoveAnimation>().PlayIn();
        }
    }


    public void showDarkBackGround()
    {
        darkBackground.show();
    }
    
    public void hideDarkBackGround()
    {
        darkBackground.hide();
    }

    public void showNewHighScore()
    {
        if (!newHighScoreIsAnimating)
        {
            newHighScoreIsAnimating = true;
            StartCoroutine("animateNewHighScore");
            
        }
    }

    public void stopAnimatingNewHighScore()
    {
        Color clr = newHighScoreFire.color;
        newHighScoreIsAnimating = false;
        newHighScoreFire.color = new Vector4(clr.r, clr.g, clr.b, 0);
        StopCoroutine("animateNewHighScore");
    }

    private IEnumerator animateNewHighScore()
    {
        Color clr = newHighScoreFire.color;
        float step = 0.05f;
        while (true)
        {
            while (newHighScoreFire.color.a < 1)
            {
                newHighScoreFire.color = new Vector4(clr.r, clr.g,clr.b,newHighScoreFire.color.a + step);
                yield return new WaitForSeconds(step);
            }
            yield return new WaitForSeconds(step);
            while (newHighScoreFire.color.a > 0)
            {
                newHighScoreFire.color = new Vector4(clr.r, clr.g,clr.b,newHighScoreFire.color.a - step);
                yield return new WaitForSeconds(step);
            }
            yield return new WaitForSeconds(step);
        }
    }

    public void showTutorial()
    {
        tutorial.SetActive(true);
        //GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayOut();
    }
    
    public void hideTutorial()
    {
        tutorial.SetActive(false);
        GameObject.Find("StartButton").GetComponent<MoveAnimation>().PlayIn();
    }
}

