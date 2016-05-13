using UnityEngine;

[RequireComponent(typeof(playerMotor))]
public class playerController : MonoBehaviour {
    [SerializeField]
    private float speed = 5f; //movement speed
    [SerializeField]
    private float lookSensitivity = 3f; //look sensitivity....

    /* DISABLED
    [SerializeField]
    private float thrusterForce = 1000f; 
    */

    [SerializeField]
    private LayerMask envMask;

    /*
    [Header("Spring Settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;
    */

    //my components
    private playerMotor myMotor;
    //private configurableJoint joint;
    
    void Start()
    {
        myMotor = GetComponent<playerMotor>();
    }

    void Update()
    {
        //calculate movements
        float _xMov = Input.GetAxisRaw("Horizontal"); //-1 to 1
        float _zMov = Input.GetAxisRaw("Vertical"); //forward and back

        //Add to our local transform right
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;


        //add our two movements together, Normalize it to length of 1 to get a constant speed
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //send to our motor
        myMotor.getMovement(_velocity);

        //Calculate our body rotation as a 3D vector
        //get our rotation first
        float _yRot = Input.GetAxisRaw("Mouse X");
        //store our rotation in a vector
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;
        //send it to the motor
        myMotor.getRotation(_rotation);


        //calculate our camera rotation
        //get our rotation
        float _xRot = Input.GetAxisRaw("Mouse Y");
        //store our rotation
        float _cameraRotationX = _xRot * lookSensitivity;
        //send to motor
        myMotor.getCameraRotation(_cameraRotationX);

    }

}
