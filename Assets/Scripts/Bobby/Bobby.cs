using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bobby : MonoBehaviour
{
    private PlayerInputs Inputs;

    [Header("Player")]
    private Rigidbody rb;
    public int hp = 3;
    public bool isRunning;
    public bool nearClimb = false;
    [SerializeField]private bool isGrounded;
    
    [Header("Movimiento")]
    public float speed = 5.0f;
    public float jumpForce = 15f;
    public float speedMultiplier = 1.0f;
    public float turnSmooth = 0.1f;
    private float turnSmoothVelocity;
    private Vector3 moveDir;
    private Vector3 currentSpeed;

    [Header("Grab&Throw")]
    public bool nearObject = false;
    public Rigidbody item = null;
    public Transform objectHolder;
    public float throwForce;
    public bool carryObject = false;
    public bool isTrowable = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Inputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        /// ---- Movement ----- ///
        Vector3 currentSpeed = rb.velocity;
        Vector2 inputVector = Inputs.dirInput.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y).normalized;

        if (direction.magnitude >= 0.1)//Existe un input de movimiento
        {
            if (Inputs.runInput.triggered)
            {
                isRunning = true;
                speedMultiplier = 1.5f;
            }

            //Rotacion
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Direccion 
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentSpeed = moveDir * speed * speedMultiplier;
        }
        else
        {
            currentSpeed *= 0.95f;
            speedMultiplier = 1f;
            isRunning = false;
        }

        //Salto
        if (Inputs.jumpInput.triggered && isGrounded)
            currentSpeed.y = jumpForce;
        else
            currentSpeed.y = rb.velocity.y;

        //Actualizacion de velocidad
        rb.velocity = currentSpeed;


        /// ---- Drag & Drop ----- ///
        
        //Recoge el objeto 
        if (nearObject && !carryObject && Inputs.interactInput.triggered)
        {
            carryObject = true;
            isTrowable = true;
            
            if (item != null)
            {
                
                item.transform.position = objectHolder.position + rb.transform.forward;
                item.transform.SetParent(objectHolder);
                item.isKinematic = true;
                item.useGravity = false;
            }
        }

        if (Inputs.throwInput.triggered && carryObject)
        {
            if (isTrowable)
            {
                Drop();
                //Debug.DrawRay(item.transform.position, transform.forward, Color.red, 10f);
                item.AddForce(transform.forward  * throwForce);
            }
            else
                Drop();
        }


        if (nearClimb) //Work In Progress
            return;
    }

    public void TakeDamage() => hp--;

    public void Drop()
    {
        carryObject = false;
        isTrowable = false;

        objectHolder.DetachChildren();
        item.isKinematic = false;
        item.useGravity = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !isGrounded)
            isGrounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8 && isGrounded)
            isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gryppy")
        {
            nearObject = true;
            item = other.GetComponent<Rigidbody>();
        }

        if (other.tag == "Climbable")
            nearClimb = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gryppy")
        {
            nearObject = false;
            item = null;
        }

        if (other.tag == "Climbable")
            nearClimb = false;
    }

}
