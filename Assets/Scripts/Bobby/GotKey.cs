using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotKey : MonoBehaviour
{
    [SerializeField]
    private bool gotKey;
    [SerializeField]
    private Animation doorAnim;

    void Start()
    {
        gotKey = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("key"))
        {
            Destroy(other.gameObject);
            gotKey = true;
        }
        else if (other.CompareTag("door"))
        {
            doorAnim.Play();
            Win();
        }
    }


    private void Win()
    {
        //go to win screen
        Debug.Log("You win!");
    }

    
}
