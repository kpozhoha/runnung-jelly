using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Jelly))]
public class ScoreStreak : MonoBehaviour
{
    public ScoreAnimationAnimator phrasesCanvas;
    [SerializeField] private Material material;
    [SerializeField]
    private string[] inspirationPhrases;
    [SerializeField]
    private Color[] phrasesColors;
    [SerializeField]
    private int godModeLength;
    private int jumpsInGodMode;

    [SerializeField] private int jumpsToStartGodMode;
    [SerializeField] private GameField gameField;
    private FieldController fieldController;
    public bool inGodMode = false;
    private float cashedTimeScale;

    [SerializeField]
    private AudioClip[] godModeSounds;
    private AudioSource godModeAudio;

    [SerializeField] private AudioClip[] sounds;
    public int Streak {
        get {
            return scoreStreak;
        }
    }

    public int scoreStreak;
    private Vector3 phrasesCanvasPosition;

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponentInParent<AudioSource>();
        godModeAudio = gameObject.AddComponent<AudioSource>();
        godModeAudio.volume = audio.volume;
        fieldController = gameField.GetComponent<FieldController>();
        if (phrasesColors.Length != inspirationPhrases.Length) {
            throw new Exception("phrasesColors and inspirationPhrases arrays must have the same length");
        }
        scoreStreak = 0;
        phrasesCanvasPosition = phrasesCanvas.transform.position;
        material.SetColor("_Color1", new Color32(255, 0, 60, 255));
        material.SetColor("_Color2", new Color32(0, 255, 243,255));
    }

    public void RecentTileWasDestroyed(bool wasDestroyed) {
        if (wasDestroyed) {
            scoreStreak++;
            audio.clip = sounds[Math.Min(scoreStreak, sounds.Length - 1)];
            audio.Play();
            if (scoreStreak == jumpsToStartGodMode && !inGodMode)
            {
                startGodMode();
            }
            phrasesCanvas.transform.position = phrasesCanvasPosition;
            phrasesCanvas.play(
                inspirationPhrases[Mathf.Min(scoreStreak - 1, inspirationPhrases.Length - 1)],
                phrasesColors[Mathf.Min(scoreStreak - 1, phrasesColors.Length - 1)]
                );
            
        } else {
            scoreStreak = 0;
        }
        if (inGodMode && jumpsInGodMode++ > godModeLength) {
            endGodMode();
        }
    }

    public void ResetStreak() {
        scoreStreak = 0;
    }

    public Color GetPhraseColor(int phraseNumber) {
        return phrasesColors[Math.Min(phraseNumber - 1, phrasesColors.Length - 1)];
    }


    public void startGodMode()
    {
        godModeAudio.clip = godModeSounds[0];
        godModeAudio.Play();
        ///// breathtaking animations and fucking epelepsia///
        cashedTimeScale = GameField.getTimeScale();
        GameField.setTimeScale(Math.Min(GameField.getTimeScale() + 0.5f, 4));
        GetComponentInParent<Jelly>().inGodModParticles.Play();
        StartCoroutine(colorChange(material.GetColor("_Color1"), new Color(255f / 255, 0f / 255, 131f / 255, 255f / 255),"_Color1") );
        StartCoroutine(colorChange(material.GetColor("_Color2"), new Color(255f / 255, 107f / 255, 0f / 255, 255f / 255),"_Color2") );
        //material.SetColor("_Color1", new Color32(255, 0, 131, 255));
        //material.SetColor("_Color2", new Color32(255, 107, 0,255));
        inGodMode = true;
        jumpsInGodMode = 0;
        fieldController.godModeRegenerate();
    }

    public void endGodMode()
    {
        godModeAudio.clip = godModeSounds[1];
        godModeAudio.Play();
        GameField.setTimeScale(cashedTimeScale);
        GetComponentInParent<Jelly>().inGodModParticles.Stop();
        StartCoroutine(colorChange(material.GetColor("_Color1"), new Color(255f / 255, 0f / 255, 60f / 255, 255f / 255), "_Color1"));
        StartCoroutine(colorChange(material.GetColor("_Color2"), new Color(0f / 255, 255f / 255, 243f / 255, 255f / 255), "_Color2"));
        //material.SetColor("_Color1", new Color32(255, 0, 60, 255));
        //material.SetColor("_Color2", new Color32(0, 255, 243,255));
        inGodMode = false;
    }
    
    private IEnumerator colorChange(Color color1, Color color2,string colorName)
    {
        float time = 0.15f;
        float timeStep = 0.01f;
        Vector4 step = new Vector4();
        step = (color2 - color1) / (time / timeStep);
        Color color = new Color(color1.r, color1.g, color1.b, color1.a);
        float timeCopy = time;
        while (timeCopy > 0)
        { 
            timeCopy -= timeStep;
            color.r += step.x;
            color.g += step.y;
            color.b += step.z;
            color.a += step.w;
            material.SetColor(colorName, color);
            yield return new WaitForSeconds(timeStep);
        }
       material.SetColor(colorName, color2);
    }
    
}
