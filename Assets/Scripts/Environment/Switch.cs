using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{

    [SerializeField]
    Transform platform;
    [SerializeField]
    Collider wall, floor;
    Animation anim;
    bool on = false, action = false;
    MovingPlatform movePlatform;
    public MovableWall moveWall;
    [SerializeField]
    private PlayerInputs playerInputs;


    void Start()
    {
        Physics.IgnoreCollision(wall, GetComponent<Collider>());
        Physics.IgnoreCollision(floor, GetComponent<Collider>());
        anim = gameObject.transform.GetComponentInParent<Animation>();
        movePlatform = platform.gameObject.GetComponent<MovingPlatform>();
    }

    void MoveSwitch()
    {
        if (!on)
        {
            anim["SwitchDown"].time = 0;
            anim["SwitchDown"].speed = 1;
            on = true;
            movePlatform.MovePlatform();
           

        }
        else
        {
            Debug.Log("detect on");
            anim["SwitchDown"].time = anim["SwitchDown"].length;
            anim["SwitchDown"].speed = -1;
            on = false;
            movePlatform.StopPlatform();

        }
        anim.Play();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            
            action = true;
        }  
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            action = false;
        }
    }

    private void Update()
    {
        if (action &&  playerInputs.interactInput.triggered)  
        {
            Debug.Log("switch");
            MoveSwitch();
            moveWall.ActivateWall();
        }
    }
}
