using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodAnimation : MonoBehaviour
{
    public float verticalMovementSpeed = Mathf.PI / 36;
    public float rotationSpeed = 0.5f;
    public float amplitude = 0.0005f;

    private float angle = 0;

    private float finalAngle;
    private bool shouldStop = false;

    public void RotateToAngleAndStop(float yAngle) {
        finalAngle = yAngle;
        shouldStop = true;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 position = transform.localPosition;
        position.y += -Mathf.Sin(angle - verticalMovementSpeed) * amplitude + Mathf.Sin(angle) * amplitude;
        angle += verticalMovementSpeed;
        transform.localPosition = position;

        Vector3 angles = transform.localEulerAngles;
        // speed of rotation if shouldStop == true, 
        //should be big to perform rotation to final rotation in small time interval
        const float SPEED_BEFORE_STOP = 4f;
        if (shouldStop) {
            angles.y += SPEED_BEFORE_STOP;
            if (Mathf.Abs(angles.y - finalAngle) <= SPEED_BEFORE_STOP / 2) {
                angles.y = finalAngle;
                transform.localEulerAngles = angles;
                shouldStop = false;
                enabled = false;
            } 
        } else {
            angles.y += rotationSpeed;
        }
        transform.localEulerAngles = angles;
    }
}
