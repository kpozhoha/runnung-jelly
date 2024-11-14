using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FlashingAnimation : MonoBehaviour
{
    [SerializeField]
    private float maxAlphaValue;
    [SerializeField]
    private float showTime;
    [SerializeField]
    private float hideTime;

    private SpriteRenderer img;

    // Start is called before the first frame update
    void Start() {
        img = GetComponent<SpriteRenderer>();
    }

    public void Play() {
        StartCoroutine(nameof(Show));

    }

    private IEnumerator Show() {
        float timeStep = showTime * (Time.timeScale / GameField.MIN_TIMESCALE) / 10;
        float alphaStep = maxAlphaValue / 10;
        float time = 0;
        Color color = img.color;
        while (time < showTime * (Time.timeScale / GameField.MIN_TIMESCALE)) {
            time += timeStep;
            color.a += alphaStep;
            img.color = color;
            yield return new WaitForSeconds(timeStep);
        }
        color.a = maxAlphaValue;
        img.color = color;
        StartCoroutine(nameof(Hide));
    }

    private IEnumerator Hide() {
        float timeStep = hideTime * (Time.timeScale / GameField.MIN_TIMESCALE) / 10;
        float alphaStep = maxAlphaValue / 10;
        float time = 0;
        Color color = img.color;
        while (time < hideTime * (Time.timeScale / GameField.MIN_TIMESCALE)) {
            time += timeStep;
            color.a -= alphaStep;
            img.color = color;
            yield return new WaitForSeconds(timeStep);
        }
        color.a = 0;
        img.color = color;
    }
}
