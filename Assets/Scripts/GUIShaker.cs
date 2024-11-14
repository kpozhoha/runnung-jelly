using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIShaker : MonoBehaviour
{
    [SerializeField] GameObject[] gui;
    [SerializeField]
    private float step = 0.02f;
    
    public void play()
    {
        StartCoroutine(animate());
    }

    private IEnumerator animate()
    {
        const float DELTA_TIME = 0.01f;
        foreach (var element in gui)
        {
            element.transform.position = new Vector3(element.transform.position.x + step, element.transform.position.y, element.transform.position.z);
        }
        yield return new WaitForSeconds(DELTA_TIME);
        foreach (var element in gui)
        {
            element.transform.position = new Vector3(element.transform.position.x - step, element.transform.position.y, element.transform.position.z);
        }
        yield return new WaitForSeconds(DELTA_TIME);
        foreach (var element in gui)
        {
            element.transform.position = new Vector3(element.transform.position.x - step, element.transform.position.y, element.transform.position.z);
        }
        yield return new WaitForSeconds(DELTA_TIME);
        foreach (var element in gui)
        {
            element.transform.position = new Vector3(element.transform.position.x + step, element.transform.position.y, element.transform.position.z);
        }
        yield return new WaitForSeconds(DELTA_TIME);
        
        // gui.transform.position = new Vector3(gui.transform.position.x - step, gui.transform.position.y, gui.transform.position.z);
        // yield return new WaitForSeconds(0.5f);
        // gui.transform.position = new Vector3(gui.transform.position.x - step, gui.transform.position.y, gui.transform.position.z);
        // yield return new WaitForSeconds(0.5f);
        // gui.transform.position = new Vector3(gui.transform.position.x + step, gui.transform.position.y, gui.transform.position.z);
        // yield return new WaitForSeconds(0.5f);
    }
}
