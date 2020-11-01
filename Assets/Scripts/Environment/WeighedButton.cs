using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighedButton : MonoBehaviour
{
    [SerializeField]
    private int blockCount;
    private Animation anim;
    private bool activated;
    public Animation bridgeMovement;

    void Start()
    {
        blockCount = 0;
        anim = GetComponentInParent<Animation>();
    }

    private void DeactivateButton()
    {
        anim["WeighedButton"].time = anim["WeighedButton"].length;
        anim["WeighedButton"].speed = -1;
        activated = false;
        anim.Play();

        bridgeMovement["BridgeMove"].time = bridgeMovement["BridgeMove"].length;
        bridgeMovement["BridgeMove"].speed = -1;
        bridgeMovement.Play();

        Debug.Log("Deactivated!");

    }

    private void ActivateButton()
    {
        Debug.Log(anim, anim);
        Debug.Log(anim["WeighedButton"]);
        anim["WeighedButton"].time = 0;
        anim["WeighedButton"].speed = 1;
        anim.Play();


        bridgeMovement["BridgeMove"].time = 0;
        bridgeMovement["BridgeMove"].speed = 1;
        bridgeMovement.Play();

        
        activated = true;
        Debug.Log("Activated!");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            blockCount++;
            Debug.Log("in platform " + blockCount);
            if (blockCount == 3) ActivateButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("block"))
        {
            blockCount--;
            Debug.Log("out platform " + blockCount);
            if (activated && blockCount < 3) DeactivateButton();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
