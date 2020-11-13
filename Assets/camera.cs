using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float smoothing = 2;

    Vector3 offset; 
 // Use this for initialization
 void Start()
    {
        offset = transform.position - target.transform.position;
    }
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.fixedDeltaTime);
    }

}

