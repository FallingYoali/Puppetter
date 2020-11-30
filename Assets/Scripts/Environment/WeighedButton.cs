using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeighedButton : MonoBehaviour
{
    [SerializeField]
    private int blockCount;
    private Animation anim;
    private bool activated;
    public Animator animatorButton;
    public Animation animBox;

    void Start()
    {
        blockCount = 0;
    }

    private void ActivateButton()
    {
        animBox.Play();
        Debug.Log("Activated!");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("block"))
        {
            blockCount++;
            animatorButton.SetInteger("boxes", blockCount);
            Debug.Log("in platform " + blockCount);
            if (blockCount == 4) ActivateButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("block"))
        {
            blockCount--;
            Debug.Log("out platform " + blockCount);
        }
       
    }
}
