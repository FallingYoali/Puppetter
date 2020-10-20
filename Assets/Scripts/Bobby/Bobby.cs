using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobby : MonoBehaviour
{
    [Header("Player")]
    public float speed = 15.0f;
    public float speedMultiplier = 1.0f;
    public float turnSmooth = 0.1f;
    public float jumpSpeed = 5f;
    public float gravity = 2f;
    public int hp = 3;
    [SerializeField] private bool isGrounded;

    private float turnSmoothVelocity;
    private CharacterController playerController;
    private Vector3 moveDir;

    private void Awake() => playerController = GetComponent<CharacterController>();

    private void Update()
    {
        
    }


    void MoveCharacterController()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal * speedMultiplier, 0f, vertical * speedMultiplier).normalized;

        if (direction.magnitude >= 0.1f)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                speedMultiplier = 1.5f;

            //Rotacion
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            //Posicion 
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerController.Move(moveDir.normalized * speed * Time.deltaTime);

            isGrounded = playerController.SimpleMove(moveDir);


        }
        else
        {
            speedMultiplier = 1f;
        }
    }


    void TakeDamage() => hp--;
}
