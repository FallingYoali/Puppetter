using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    private Animation bounceAnim;
    [SerializeField]
    private int thrust;

    private void Start()
    {
        bounceAnim = GetComponent<Animation>();
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * thrust);
            bounceAnim.Play();
        }
    }
}
