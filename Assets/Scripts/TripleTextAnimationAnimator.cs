using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleTextAnimationAnimator : MonoBehaviour
{
    public Vector3 startPosition;
    [SerializeField] private float time;
    private float timeStep = 0.01f;
    private Vector3 step = new Vector3();
    [SerializeField] private TripleText[] textArray;
    private int activeText;
    private float opacityStep;

    private void Start() {
        activeText = 0;
    }

    public void play(string str, Color color)
    {
        StopCoroutine(animate());
        TripleText text = textArray[activeText];
        //text.InnerColor = color;
        text.Text = str;
        text.GeneralOpacity = 1;
        startPosition = transform.position;
        step = new Vector3(0,0.07f,0);
        opacityStep = 1 / (time / timeStep);
        StartCoroutine("animate");
    }
    
    
    public IEnumerator animate()
    {
        TripleText text = textArray[activeText];
        activeText = (activeText + 1) % textArray.Length;
        float timeCopy = time;
        while (timeCopy > 0)
        {
            timeCopy -= timeStep;
            text.GeneralOpacity = text.GeneralOpacity - opacityStep;
            transform.localPosition += step;
            yield return new WaitForSeconds(timeStep);
        }
        text.GeneralOpacity = 0;
    }
}
