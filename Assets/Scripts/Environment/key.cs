using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    [SerializeField]
    private KeyManager keys;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            keys.GrabKey();
            Destroy(this.gameObject);
        }
    }


}
