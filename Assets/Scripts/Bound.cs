using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    public string boundName;
    private BoxCollider2D bound;
    private CameraManager theCamera;
    void Start()
    {
        bound = GetComponent<BoxCollider2D>();
        theCamera = FindObjectOfType<CameraManager>();
        theCamera.SetBound(bound);
    }
    public void SetBound()
    {
        if(theCamera != null)
        {
            theCamera.SetBound(bound);
        }
    }
}
