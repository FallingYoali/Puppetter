using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField]
    private int keyCounter;
    [SerializeField]
    private Animation doorOpen;


    void Start()
    {
        keyCounter = 0;
    }

    public void GrabKey()
    {
        keyCounter++;
        if (keyCounter == 5)
        {
            Debug.Log("All keys have been taken! " + doorOpen);
            doorOpen.Play();
        }
    }

}
