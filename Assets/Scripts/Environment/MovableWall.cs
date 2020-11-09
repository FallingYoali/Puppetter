using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableWall : MonoBehaviour
{
    private Animation moveWall;
    void Start()
    {
        moveWall = GetComponent<Animation>();
    }

    public void ActivateWall()
    {
        moveWall.Play();
    }
}
