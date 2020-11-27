using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bobby : MonoBehaviour
{
    private PlayerInputs Inputs;

    [Header("Player")]
    private Rigidbody playerRb;
    public int hp = 3;
    [SerializeField] private bool isGrounded;
    [SerializeField] GameObject espadita;
    [SerializeField] private Camera mainCamera;
    

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
    private bool nearObject = false;
    private Rigidbody item = null;
    public Transform objectHolder;
    public float throwForce;
    public bool carryObject = false;
    public bool isTrowable = false;

    [Header("Climbing")]
    public bool nearClimb = false;
    public bool isClimbing = false;
    public GameObject wall;
    public float climbSpeed = 2f;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        Inputs = GetComponent<PlayerInputs>();
    }

    private void Update()
    {
        /// ---- Movement ----- ///
        currentSpeed = playerRb.velocity;
        Vector2 inputVector = Inputs.dirInput.ReadValue<Vector2>();
        Vector3 direction = new Vector3(inputVector.x, 0f, inputVector.y).normalized;

        if (direction.magnitude >= 0.1)//Existe un input de movimiento
        {
            currentSpeed = Movement2(direction);
        }
        else //Dejo de moverse
        {
            currentSpeed *= 0.95f;
            speedMultiplier = 1f;
            //isRunning = false;
        }

        if(Inputs.attackInput.triggered)
        {
            espadita.SetActive(true);
            Invoke(nameof(DesactivateEspadita), 0.5f);
        }

        //Salto
        if (Inputs.jumpInput.triggered && isGrounded)
            currentSpeed.y = jumpForce;
        else
            currentSpeed.y = playerRb.velocity.y;


        //Actualizacion de velocidad
        playerRb.velocity = currentSpeed;

        /// ---- Drag & Drop ----- ///

        //Recoge el objeto 
        if (nearObject && !carryObject && Inputs.interactInput.triggered)
        {
            carryObject = true;
            isTrowable = true;

            if (item != null)
            {

                item.transform.position = objectHolder.position + playerRb.transform.forward * 0.3f;
                item.transform.SetParent(objectHolder);
                item.isKinematic = true;
                item.useGravity = false;
            }
        }

        //Lo suelta o lo lanza
        if (Inputs.throwInput.triggered && carryObject)
        {
            if (isTrowable)
                Throw();
            else
                Drop();
        }


        /// ---- Climbing ---- ///

        if (isClimbing) //
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
                playerRb.useGravity = false;
                isClimbing = true;
                wall = hit.collider.gameObject;
                playerRb.position = wall.transform.position + (wall.transform.right * 0.5f) + new Vector3(0f, 0f, -0.75f);
                //playerRb.transform.LookAt(new Vector3(wall.transform.position.x, playerRb.transform.position.y, wall.transform.position.z - 0.75f));
            }
        }

        /// ---- Falling dmg ---- ///
        if (!isGrounded)
            FallingDmg();

    }


    private Vector3 Movement(Vector3 _direction){
        if (isClimbing) //Si esta escalando no se tiene que mover de manera normal
            return Vector3.zero;

        if (Inputs.runInput.triggered)//Sprint
        {
            //isRunning = true;
            speedMultiplier = 1.5f;

        }
        //Rotacion
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg; //Retorna angulo hacia donde se va a mover
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth); 
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //Direccion 
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        return moveDir * speed * speedMultiplier;
    }

    private Vector3 Movement2(Vector3 _direction){
        if(isClimbing)
            return Vector3.zero;

        if(Inputs.runInput.triggered)
            speedMultiplier = 1.5f;

        //Toma los vectores hacia donde mira la camara
        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;
        //Debug.DrawRay(mainCamera.transform.position, camForward, Color.red, 2f);

        //Toma los inputs con respecto a la posicion de la camara
        moveDir = (camForward * _direction.z) + (camRight * _direction.x);

        //Rotacion
        float targetAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;//Retorna angulo hacia donde se va a mover
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmooth, turnSmooth);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        //Direccion
        return moveDir * speed * speedMultiplier;       
    }

    private void DesactivateEspadita()
    {
        espadita.SetActive(false);
       
    }

    public void TakeDamage(int value)
    {
        hp -= value;
        if(hp<= 0)
        {
            gameObject.SetActive(false);
        }

    }

    private void Throw(){
        item.isKinematic = false;
        item.useGravity = true;
        carryObject = false;
        isTrowable = false;

        objectHolder.DetachChildren(); 
        item.AddForce(transform.forward * throwForce);
    }    

    public void Drop()
    {
        item.isKinematic = false;
        item.useGravity = true;
        carryObject = false;
        isTrowable = false;

        objectHolder.DetachChildren(); 
    }

    private void Climb() //Necesita revision
    {
        Vector2 inputVector = Inputs.dirInput.ReadValue<Vector2>();

        if (inputVector.magnitude >= 0.1 && wall.name == "Stairs")//Existe un input de movimiento y es una escalera
        {
            currentSpeed.y = inputVector.y * climbSpeed;
            currentSpeed += playerRb.transform.forward * climbSpeed;
        }
        else
            currentSpeed *= 0.85f;


        if(Inputs.interactInput.triggered)//Deja de escalar
        {
            wall = null;
            isClimbing = false;
            playerRb.useGravity = true;
            return;
        }

        playerRb.velocity = currentSpeed;
    }

    private void FallingDmg(){
        RaycastHit hit;
        Ray dirRay = new Ray(transform.position, -transform.up);
        float distanceToGround;

        if (takeFallingDmg)//Verifica si ya tiene marcado que recibe dmg
            return;

        if (Physics.Raycast(dirRay, out hit) && hit.collider.gameObject.layer == 8)
        {
            distanceToGround = transform.position.y - hit.transform.position.y;
            if (distanceToGround >= 15f)
                takeFallingDmg = true;

        }
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
        if (other.tag == "Gryppy" || other.tag == "block")
        {
            nearObject = true;
            item = other.GetComponent<Rigidbody>();
        }

        if (other.tag == "BeginClimb")
            nearClimb = true;
        
        if(isClimbing && other.tag == "EndClimb")
        {
            playerRb.useGravity = true;
            wall = null;
            isClimbing = false;
            playerRb.transform.position += playerRb.transform.forward + new Vector3(0f, 1f, 0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gryppy" || other.tag == "block")
        {
            nearObject = false;
            item = null;
        }

        if (other.tag == "BeginClimb")
            nearClimb = false;
    }

}
