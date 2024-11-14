using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpinLabelAnimator : MonoBehaviour
{
    public void play()
    {
        StopCoroutine(animateAppearence());
        StartCoroutine(animateAppearence());
    }
    
    private IEnumerator animateAppearence()
    {
        
        while (true)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 15); 
            yield return new WaitForSeconds(0.1f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 15);
            yield return new WaitForSeconds(0.1f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 15);
            yield return new WaitForSeconds(0.1f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 15);
            yield return new WaitForSeconds(2);
        }
    }
}
