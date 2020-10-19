using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField]
    private float low_y, high_y, speed;  //how high and low the platform will reach
    private float rateVelocity, time;
    private Vector3 startPos, endPos;
    private bool on, goingDown;

    private void Start()
    {
        
    }
    public void MovePlatform()
    {
        Debug.Log("platform moving");
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, low_y, transform.position.z);
        
        rateVelocity = 1f / Vector3.Distance(startPos, endPos) * speed;
        
        time = 0.0f;
        goingDown = true;
        on = true;
    }

    public void StopPlatform()
    {
        on = false;
        time = 0.0f;
    }

    private void Update()
    {
        if (on)
        {
            
            if (time >= 0.1f) //this works well with a 0.1 speed
            {
                goingDown = !goingDown;
                time = 0.0f;
                Debug.Log("Down: " + goingDown);
            }
            if (time <= 1.0f)
            {
                if (goingDown)
                {
                    startPos = transform.position;
                    endPos = new Vector3(transform.position.x, low_y, transform.position.z);
                }
                else
                {
                    startPos = transform.position;
                    endPos = new Vector3(transform.position.x, high_y, transform.position.z);
                }
                time += Time.deltaTime * rateVelocity;
                transform.position = Vector3.Lerp(startPos, endPos, time);
            }        
        }
    }
}
