using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeAnimation : MonoBehaviour
{
    public bool IsRunning {
        get {
            return isRunning;
        }
    }
    private bool isRunning;
    [SerializeField]
    private float height;
    [SerializeField]
    private float rotationSpeed;

    private void Start() {
        isRunning = false;
    }

    public void Animate() {
        transform.position = transform.position + new Vector3(0, 0.1f, 0);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null && !isRunning) {
            rb.isKinematic = true;
            rb.isKinematic = false;
            rb.AddForce(new Vector3(0, 1, 0) * height, ForceMode.VelocityChange);
            rb.AddTorque(new Vector3(0, 1, 0) * rotationSpeed, ForceMode.VelocityChange);
        }
        isRunning = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag.Equals("Tile")) {
            isRunning = false;
        }
    }
}
