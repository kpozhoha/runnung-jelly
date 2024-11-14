using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkBackgroundAnimation : MonoBehaviour
{
    private Image img;
    [SerializeField] private float time;
    [SerializeField] private float darknessPercent;
    private const float timeStep = 0.01f;
    private float step;
    void Awake()
    {
        img = GetComponent<Image>();
        img.color = new Vector4(img.color.r,img.color.g, img.color.b,0);
        step = darknessPercent/(time/timeStep);
    }

    public void show()
    {
        StartCoroutine("showAnim");
    }

    public void hide()
    {
        StartCoroutine("hideAnim");
    }
    
    IEnumerator showAnim()
    {
        float timeCopy = time;
        while (timeCopy > 0)
        {
            img.color = new Vector4(img.color.r,img.color.g, img.color.b,img.color.a + step);
            timeCopy -= timeStep;
            yield return new WaitForSeconds(timeStep);
        }
       img.color = new Vector4(img.color.r,img.color.g, img.color.b,darknessPercent);
    }
    
    IEnumerator hideAnim()
    {
        float timeCopy = time;
        while (timeCopy > 0)
        {
            img.color = new Vector4(img.color.r,img.color.g, img.color.b,img.color.a - step);
            timeCopy -= timeStep;
            yield return new WaitForSeconds(timeStep);
        }
        img.color = new Vector4(img.color.r,img.color.g, img.color.b,0);
    }
}
