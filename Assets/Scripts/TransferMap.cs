using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    public Transform target;
    public BoxCollider2D targetBound;
    private PlayerManager thePlayer;
    private CameraManager theCamera;

    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player(Main)")
        {
            theCamera.SetBound(targetBound);
            thePlayer.currentMapName = transferMapName;
            thePlayer.transform.position = target.transform.position;
            theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        }
    }
}
