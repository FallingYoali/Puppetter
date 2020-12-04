using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaFollowCamara : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    [SerializeField] Camera cam;

   
    void Update()
    {

        transform.forward = cam.transform.forward;

    }
}
