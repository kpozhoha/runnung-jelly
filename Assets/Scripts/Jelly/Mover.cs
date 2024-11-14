using UnityEngine;
using System;

public class Mover : MonoBehaviour
{
    public float distanceBetweenTileCenters = 5.5f;
    
    [Tooltip("time in seconds")]
    public float durationOfJump = 1;
    [Tooltip("time in frames")]
    public float bounceDuration = 25;

    public static Vector3 Forces;

    
    private Jelly jelly;
    private Mover mover;
    private Rigidbody rb;
    public Vector3 Direction {
        get;
        set;
    }

    // contains one non-zero component to scale up/down
    private Vector3 bouncingAxis;
    // scale of squeeze
    [Tooltip("scale of squeeze")]
    public double scale = 0.1;
    // scale change each frame
    private double deltaAngle = 2 * Math.PI / 25;
    private double angle;

    // true if bouncing animation is active
    public bool IsBouncing { get; set; }

    private bool isResetting = false;

    private void Start() {
        Forces = new Vector3(
            distanceBetweenTileCenters / durationOfJump, 
            Math.Abs(Physics.gravity.y) * durationOfJump / 2,
            distanceBetweenTileCenters / durationOfJump);

        angle = 0;
        deltaAngle = 2 * Math.PI / bounceDuration;
        IsBouncing = false;

        Direction = new Vector3(0, Forces.y, 0);
        rb = GetComponent<Rigidbody>();
        if (rb != null) {    
            rb.freezeRotation = true;
        }
        
       jelly = gameObject.GetComponent<Jelly>();
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        //if (IsBouncing) {
        //    if (angle >= 2 * Math.PI) {
        //        angle = 0;
        //        if (jelly != null) {
        //            jelly.OnBouncingFinished();
        //        }

        //        if (!isResetting) {
        //            Move();
        //        } else {
        //            isResetting = false;
        //        }

        //        IsBouncing = false;
        //    }

        //    transform.localScale += -(float)Math.Sin(angle) * bouncingAxis * (float)scale;
        //    angle += deltaAngle;
        //}
    }

    public void Move() {
        //Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) {
            rb.ResetInertiaTensor();
            rb.velocity = Vector3.zero;
            rb.AddForce(Direction, ForceMode.VelocityChange);
            Vector3 torque = new Vector3(Direction.z, 0, -Direction.x);
            torque = torque.normalized;
            rb.AddTorque(torque * (float)Math.PI / (durationOfJump * 2), ForceMode.VelocityChange);
        }
    }

    //public void StartBounce(Vector3 bounceVector) {
    //    bouncingAxis = bounceVector;
    //    IsBouncing = true;
    //}

    public void Reset() {
        Direction = new Vector3(0, Forces.y, 0);
        isResetting = true;
    }
}
