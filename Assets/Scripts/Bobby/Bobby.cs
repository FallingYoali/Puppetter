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
    [SerializeField] private bool isGrounded;
    [SerializeField] GameObject espadita;

    [Header("Movimiento")]
    public float speed = 5.0f;
    public float jumpForce = 15f;
    public float speedMultiplier = 1.0f;
    public float turnSmooth = 0.1f;
    private float turnSmoothVelocity;
    private Vector3 moveDir;
    private Vector3 currentSpeed;
    private bool takeFallingDmg = false;

    [Header("Grab&Throw")]
    public bool nearObject = false;
    public Rigidbody item = null;
    public Transform objectHolder;
    public float throwForce;
    public bool carryObject = false;
    public bool isTrowable = false;

    [Header("Climbing")]
    public bool nearClimb = false;
    public bool isClimbing = false;
    public GameObject wall = null;
    public float climbSpeed = 2f;
    public Vector3 distanceToWall = new Vector3(0, 0, 0);


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
            if (isClimbing) //Si esta escalando no se tiene que mover de manera normal
                return;

            if (Inputs.runInput.triggered)//Sprint
            {
                isRunning = true;
                speedMultiplier = 1.5f;
            }

            //Rotacion
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; //Retorna angulo hacia donde se va a mover
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth); //Te 
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Direccion 
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            currentSpeed = moveDir * speed * speedMultiplier;
        }
        else //Dejo de moverse
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

        //Lo suelta o lo lanza
        if (Inputs.throwInput.triggered && carryObject)
        {
            if (isTrowable)
            {
                Drop();
                //Debug.DrawRay(item.transform.position, transform.forward, Color.red, 10f);
                item.AddForce(transform.forward * throwForce);
            }
            else
                Drop();
        }


        /// ---- Climbing ---- ///


        if (isClimbing)
        { //Work In Progress 
            Climb();
            return;
        }

        if (nearClimb && Inputs.interactInput.triggered)
        {
            RaycastHit hit;
            Ray dirRay = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(dirRay, out hit, 2f) && hit.collider.tag == "Climbable") //Verifica que el player este mirando a la pared
            {

                rb.useGravity = false;
                isClimbing = true;
                wall = hit.collider.gameObject;

                //rb.rotation = Quaternion.Euler(distanceToWall);
            }
        }


        /// ---- Falling dmg ---- ///

        if (!isGrounded)
        {
            RaycastHit hit;
            Ray dirRay = new Ray(transform.position, -transform.up);
            float distanceToGround;

            if (takeFallingDmg)
                return;

            if (Physics.Raycast(dirRay, out hit) && hit.collider.gameObject.layer == 8)
            {
                distanceToGround = transform.position.y - hit.transform.position.y;
                if (distanceToGround >= 15f)
                    takeFallingDmg = true;

            }
        }
    }


    public void TakeDamage(int value)
    {
        hp -= value;
        if(hp<= 0)
        {
            gameObject.SetActive(false);
        }

    }

    public void Drop()
    {
        carryObject = false;
        isTrowable = false;

        objectHolder.DetachChildren();
        item.isKinematic = false;
        item.useGravity = true;
    }

    private void Climb() //Necesita revision
    {
        Debug.Log("Climbing");
        Vector2 inputVector = Inputs.dirInput.ReadValue<Vector2>();
        inputVector = inputVector.normalized;

        Debug.Log(inputVector.x);
        Debug.Log(inputVector.y);

        if (inputVector.magnitude >= 0.1)//Existe un input de movimiento
        {
            if (inputVector.x >= 0.1)
                currentSpeed = transform.right * climbSpeed;
            else if (inputVector.x <= -0.1)
                currentSpeed = -transform.right * climbSpeed;
            else if (inputVector.y >= 0.1)
                currentSpeed = transform.up * climbSpeed;
            else if (inputVector.y <= -0.1)
                currentSpeed = -transform.up * climbSpeed;
        }

        rb.velocity = currentSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8 && !isGrounded)
        {
            isGrounded = true;
            if (takeFallingDmg)
            {
                takeFallingDmg = false;
                TakeDamage(1);
            }
        }
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
        {
            nearClimb = true;
            wall = other.GetComponent<GameObject>();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gryppy")
        {
            nearObject = false;
            item = null;
        }

        if (other.tag == "Climbable")
        {
            nearClimb = false;
            wall = null;

        }

    }

}
