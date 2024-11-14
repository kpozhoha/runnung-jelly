// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class ExitShopButton : MonoBehaviour
// {
//     [SerializeField]
//     private GUI gui;
//     [SerializeField]
//     private Jelly jelly;
//     
//     [SerializeField]
//     private ShopButton shopButton;
//     public void exitShopButtonPressed()
//     {
//         GameObject.Find("ScoreCounter").GetComponent<MoveAnimation>().PlayIn();
//         StartCoroutine("outZoomCamera");
//         float yAngle = Mathf.Ceil(jelly.transform.rotation.eulerAngles.y / 90f) * 90f;
//         jelly.GetComponent<FoodAnimation>().RotateToAngleAndStop(yAngle);
//         Rigidbody rb = jelly.GetComponent<Rigidbody>();
//         rb.useGravity = true;
//         rb.isKinematic = false;
//         GameObject.Find("Shop").GetComponent<MoveAnimation>().PlayIn();
//         gui.setScoreCounterText("Running Jelly");
//     }
//     
//     IEnumerator outZoomCamera()
//     {
//         Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
//         float time = 0.1f;
//         float speed = 10f;
//         float journeyLength = Vector3.Distance(camera.transform.position, shopButton.getCameraOldPosition());
//         while ((camera.transform.position - shopButton.getCameraOldPosition()).magnitude >= 1f) {
//             float distCovered = (time - 0.1f) * speed;
//             float fractionOfJourney = distCovered / journeyLength;
//             time += 0.01f;
//             camera.transform.position = Vector3.Lerp(camera.transform.position, shopButton.getCameraOldPosition(), fractionOfJourney);
//             yield return new WaitForSeconds(0.01f);
//         }
//
//         camera.transform.position = shopButton.getCameraOldPosition();
//         gui.showMainMenuGUI();
//         GameObject.Find("TapToStart").GetComponent<Animator>().Play("InTapToStart");
//         
//     }
// }
