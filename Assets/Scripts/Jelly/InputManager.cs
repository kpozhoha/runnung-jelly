using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Jelly jelly;
    private Vector2 deltaPosition;
    private bool activeTouch;

    private const float MIN_SWIPE_LENGTH = 20f;

    private void OnDisable() {
        activeTouch = false;
        deltaPosition.x = 0;
        deltaPosition.y = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        Input.multiTouchEnabled = false;
        activeTouch = false;
        deltaPosition.x = 0;
        deltaPosition.y = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0) {
            deltaPosition += Input.GetTouch(0).deltaPosition;
            if (deltaPosition.magnitude >= MIN_SWIPE_LENGTH && !activeTouch) {
                activeTouch = true;
                Vector3 direction = new Vector3(0, 0, 0);
                if (Mathf.Abs(deltaPosition.x) > Mathf.Abs(deltaPosition.y)) {
                    direction.x = deltaPosition.x;
                } else {
                    direction.z = deltaPosition.y;
                }

                direction = direction.normalized;
                direction.x *= Mover.Forces.x;
                direction.z *= Mover.Forces.z;
                direction.y = Mover.Forces.y;
                Mover moving = jelly.GetComponent<Mover>();
                if (moving != null) {
                    moving.Direction = direction;
                }
            }
        } else {
            activeTouch = false;
            deltaPosition.x = 0;
            deltaPosition.y = 0;
        }

        ////// computer controller /////////
        Vector3 directionForComputer = new Vector3(0, 0, 0);
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            directionForComputer.x = 1;
            directionForComputer = directionForComputer.normalized;
            directionForComputer.x *= Mover.Forces.x;
            directionForComputer.z *= Mover.Forces.z;
            directionForComputer.y = Mover.Forces.y;
            Mover moving = jelly.GetComponent<Mover>();
            if (moving != null)
            {
                moving.Direction = directionForComputer;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            directionForComputer.x = -1;
            directionForComputer = directionForComputer.normalized;
            directionForComputer.x *= Mover.Forces.x;
            directionForComputer.z *= Mover.Forces.z;
            directionForComputer.y = Mover.Forces.y;
            Mover moving = jelly.GetComponent<Mover>();
            if (moving != null)
            {
                moving.Direction = directionForComputer;
            }
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            directionForComputer.z = 1;
            directionForComputer = directionForComputer.normalized;
            directionForComputer.x *= Mover.Forces.x;
            directionForComputer.z *= Mover.Forces.z;
            directionForComputer.y = Mover.Forces.y;
            Mover moving = jelly.GetComponent<Mover>();
            if (moving != null)
            {
                moving.Direction = directionForComputer;
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            directionForComputer.z = -1;
            directionForComputer = directionForComputer.normalized;
            directionForComputer.x *= Mover.Forces.x;
            directionForComputer.z *= Mover.Forces.z;
            directionForComputer.y = Mover.Forces.y;
            Mover moving = jelly.GetComponent<Mover>();
            if (moving != null)
            {
                moving.Direction = directionForComputer;
            }
        }
    }
}