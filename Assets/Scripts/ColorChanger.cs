using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ColorChanger : MonoBehaviour
{
    private Material material;
    private int curColor = 0;
    [SerializeField] private Color[] colors;
    [SerializeField] private float time;
    private float timeStep = 0.01f;
    private Vector3 step;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    public void changeColorToNext()
    {
        StartCoroutine(colorChange());
    }

    public void resetColor() {
        material.color = colors[0];
        curColor = 0;
    }

    private IEnumerator colorChange()
    {
        Vector4 step = (colors[curColor] - material.color)/(time/timeStep);
        float timeCopy = time;
        while (timeCopy > 0)
        { 
            timeCopy -= timeStep;
            material.color = new Color(material.color.r + step.x,material.color.g+ step.y,material.color.b + step.z,material.color.a + step.w);
            yield return new WaitForSeconds(timeStep);
        }

        material.color = colors[curColor];
        curColor++;
        curColor %= colors.Length;
    }
}
