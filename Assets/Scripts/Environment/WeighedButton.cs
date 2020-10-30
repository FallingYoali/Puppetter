using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighedButton : MonoBehaviour
{
    [SerializeField]
    private int blockCount;
    private Animation anim;
    private bool activated;

    void Start()
    {
        blockCount = 0;
        anim = GetComponentInParent<Animation>();
    }

    private void DeactivateButton()
    {
        anim["SwitchDown"].time = anim["SwitchDown"].length;
        anim["SwitchDown"].speed = -1;
        activated = false;
        anim.Play();
        Debug.Log("Deactivated!");

    }

    private void ActivateButton()
    {
        anim["SwitchDown"].time = 0;
        anim["SwitchDown"].speed = 1;
        anim.Play();
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
