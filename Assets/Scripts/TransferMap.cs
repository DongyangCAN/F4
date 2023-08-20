using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    public Transform target;
    public BoxCollider2D targetBound;
    private PlayerManager thePlayer;
    private CameraManager theCamera;
    private FadeManager theFade;
    private OrderManager theOrder;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; // 캐릭터가 바라보고 있는 방향
    private Vector2 vector; // getfloat("dirX")
    private void OnTriggerStay2D(Collider2D collision)
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
        if (collision.gameObject.name == "Player")
        {
            vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
            switch (direction)
            {
                case "UP":
                    if (vector.y == 1f)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TransforCoroutine());
                    }
                    break;
                case "DOWN":
                    if (vector.y == -1f)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TransforCoroutine());
                    }
                    break;
                case "RIGHT":
                    if (vector.x == 1f)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TransforCoroutine());
                    }
                    break;
                case "LEFT":
                    if (vector.x == -1f)
                    {
                        StopAllCoroutines();
                        StartCoroutine(TransforCoroutine());
                    }
                    break;
                default:
                    StopAllCoroutines();
                    StartCoroutine(TransforCoroutine());
                    break;
            }

        }

    }
    IEnumerator TransforCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.5f);
        theOrder.SetUnTransparent("Player");
        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);
        theOrder.Move();
    }
}
