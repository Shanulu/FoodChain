using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class playerMotor : MonoBehaviour {
    [SerializeField]
    private Camera myCam; //gives me a camera to rotate

    private Rigidbody myBody; //our rigidbody

    private Vector3 velocity = Vector3.zero; //my velocity on the horizontal plane
    private Vector3 rotation = Vector3.zero; //my rotation around my body
    private float cameraRotationX = 0f; //a value for my camera's rotation (up/down) that will be used to erect a new vector
    private float currentCameraRotationX = 0f; //will be used to clamp between our set field of view

    // disabled until i learn to fix the joint issue
    // private Vector3 thrusterForce = Vector3.zero;
    // vector for the thurster jump

    [SerializeField]
    private float cameraRotationLimit = 85f; //set a up/down limit for our camera

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }

    //get our movement vector from the controller
    public void getMovement(Vector3 _velocity) 
    {
        velocity = _velocity;
    }

    //get our rotation vector for our body
    public void getRotation(Vector3 _rotation)
    {
        rotation = _rotation;
    }

    //get our cameras rotation
    public void getCameraRotation(float _cameraRotationX)
    {
        cameraRotationX = _cameraRotationX;
    }

    //DISABLED DUE TO JOINT OBSOLETE-NESS
    /* public void getThruster (vector3 _thrusterForce) {
        thrusterForce = _thrusterForce;
    }
    */

    //Our calculations will take place during the physics update
    void FixedUpdate()
    {
        performMove();
        performRotation();
    }

    void performMove()
    {
        //make sure we are moving to avoid unnecessary calculations
        if (velocity != Vector3.zero)
        {
            myBody.MovePosition(myBody.position + velocity * Time.fixedDeltaTime);
        }
        /* if (thrusterForce != Vector3.zero) {
            rb.AddForce(thrusterForce * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
        */
    }


    void performRotation()
    {
        myBody.MoveRotation(myBody.rotation * Quaternion.Euler(rotation));
        //make sure we have a cam
        if (myCam != null)
        {
            //set our rotation and clamp it
            currentCameraRotationX -= cameraRotationX;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

            //Apply our rotation to camera
            myCam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
        }
    }
}

