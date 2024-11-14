using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimation : MonoBehaviour
{
    public Vector3 startPosition;
    [SerializeField] public Vector3 finalPosition;
    [SerializeField] private float time;
    private float timeStep = 0.01f;
    private Vector3 step = new Vector3();

    public void PlayOut()
    {
        if (startPosition == new Vector3()) {
            startPosition = transform.localPosition;
        }
        if (step == new Vector3() || step == (startPosition - finalPosition) / (time / timeStep)) {
            step = (finalPosition - startPosition) / (time / timeStep);
            StartCoroutine("animate");
        }
    }

    /// <summary>
    /// instantly moves object to final position
    /// </summary>
    public void PlayOutInstant() {
        if (startPosition == new Vector3()) {
            startPosition = transform.localPosition;
        }
        if (step == new Vector3() || step == (startPosition - finalPosition) / (time / timeStep)) {
            step = (finalPosition - startPosition) / (time / timeStep);
            transform.localPosition = finalPosition;
        }
    }
    
    public void PlayIn()
    {
        if (startPosition == new Vector3()) {
            startPosition = transform.localPosition;
        }
        if (step ==  (finalPosition - startPosition) / (time / timeStep)) {
            step = (startPosition - finalPosition) / (time / timeStep);
            StartCoroutine("animate");
        }
    }

    /// <summary>
    /// instantly moves object to start position
    /// </summary>
    public void PlayInInstant() {
        if (startPosition == new Vector3()) {
            startPosition = transform.localPosition;
        }
        if (step == (finalPosition - startPosition) / (time / timeStep)) {
            step = (startPosition - finalPosition) / (time / timeStep);
            transform.localPosition = startPosition;
        }
    }

    public IEnumerator animate()
    {
        float timeCopy = time;
        while (timeCopy > 0)
        {
            timeCopy -= timeStep;
            transform.localPosition += step;
            yield return new WaitForSeconds(timeStep);
        }
    }
    
}