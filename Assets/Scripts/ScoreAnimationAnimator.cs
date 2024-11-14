using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnimationAnimator : MonoBehaviour {
    public Vector3 startPosition;
    [SerializeField] private float time;
    private float timeStep = 0.01f;
    private Vector3 step = new Vector3();
    [SerializeField] private Text[] textArray;
    private int activeText;
    private float opacityStep;

    private void Start() {
        activeText = 0;
    }

    public void play(string str, Color color) {
        StopCoroutine(animate());
        Text text = textArray[activeText];
        text.text = str;
        text.color = new Vector4(color.r, color.g, color.b, 1);
        startPosition = transform.position;
        step = new Vector3(0, 0.07f, 0);
        opacityStep = 1 / (time / timeStep);
        StartCoroutine("animate");
    }


    public IEnumerator animate() {
        Text text = textArray[activeText];
        activeText = (activeText + 1) % textArray.Length;
        float timeCopy = time;
        while (timeCopy > 0) {
            timeCopy -= timeStep;
            text.color = new Vector4(text.color.r, text.color.g, text.color.b, text.color.a - opacityStep);
            transform.localPosition += step;
            yield return new WaitForSeconds(timeStep);
        }
        text.color = new Vector4(text.color.r, text.color.g, text.color.b, 0);
    }
}
