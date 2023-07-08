using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    private PlayerCtl thePlayer;
    public Transform target;
    private CameraManager theCamera;

    public bool isSceneTransition; // æ¿ ∂«¥¬ ∏  ¿¸»Ø º±≈√ ªÛ»≤

    void Start()
    {
        if (!isSceneTransition)
        {
            theCamera = FindObjectOfType<CameraManager>();
        }
        thePlayer = FindObjectOfType<PlayerCtl>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player(Main)")
        {
            if (isSceneTransition)
            {
                SceneManager.LoadScene(transferMapName); // æ¿ ¿¸»Ø
            }
            else
            {
                thePlayer.currentMapName = transferMapName; // ∏  ¿Ãµø
                thePlayer.transform.position = target.transform.position;
                theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
            }
        }
    }
}
