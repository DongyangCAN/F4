using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPoint; // ���� �̵�, �÷��̾ ���۵� ��ġ;
    private PlayerCtl thePlayer;
    private CameraManager theCamera;
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerCtl>();
        if(startPoint == thePlayer.currentMapName)
        {
            thePlayer.transform.position = this.transform.position;
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, theCamera.transform.position.z);
        }
    }

    void Update()
    {
        
    }
}
