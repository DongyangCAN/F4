using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FloatingText : MonoBehaviour
{
    public float moveSpeed;
    public float destoryTime;
    public Text text;
    public Vector3 vector;
    void Update()
    {
        vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);  
        text.transform.position = vector;
        destoryTime -=  Time.deltaTime;
        if(destoryTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
