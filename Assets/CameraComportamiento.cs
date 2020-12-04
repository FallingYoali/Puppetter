using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraComportamiento : PlayerInputs
{
    // Start is called before the first frame update


    public float sensibilidadVertical = 2.0f;
    public float sensibilidadHorizontal = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;


    [SerializeField] private Transform target;
    public float suavizado = 2;

    Vector3 offset;

    // Update is called once per frame

    private void Start()
    {
        offset = transform.position - target.transform.position;
    }
    void Update()
    {
        //Joystick mov Der
        Vector2 inputVector = dirInputDer.ReadValue<Vector2>();


        //Movimiento camara Mouse
        yaw += sensibilidadHorizontal * Input.GetAxis("Mouse X");
        pitch -= sensibilidadVertical * Input.GetAxis("Mouse Y");

//        yaw += sensibilidadHorizontal * inputVector.x;
  //      pitch -= sensibilidadVertical * inputVector.y;

       

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

    }

    private void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, suavizado * Time.fixedDeltaTime);
    }
}
