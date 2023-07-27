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
    public Animator anim_1;
    public Animator anim_2;
    public int door_count;
    [Tooltip("UP, DOWN, LEFT, RIGHT")]
    public string direction; // 캐릭터가 바라보고 있는 방향
    private Vector2 vector; // getfloat("dirX")
    [Tooltip("문O : true, 문X : false")]
    public bool door; // 문이 있나 없나
    void Start()
    {
        theCamera = FindObjectOfType<CameraManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theFade = FindObjectOfType<FadeManager>();
        theOrder = FindObjectOfType<OrderManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!door)
        {
            if (collision.gameObject.name == "Player(Main)")
            {
                StopAllCoroutines();
                StartCoroutine(TransforCoroutine());
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (door)
        {
            if (collision.gameObject.name == "Player(Main)")
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    vector.Set(thePlayer.animator.GetFloat("DirX"), thePlayer.animator.GetFloat("DirY"));
                    switch (direction)
                    {
                        case "UP":
                            if(vector.y == 1f)
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
        }
    }
    IEnumerator TransforCoroutine()
    {
        theOrder.PreLoadCharacter();
        theOrder.NotMove();
        theFade.FadeOut();
        if (door)
        {
            anim_1.SetBool("Open", true);
            if (door_count == 2)
            {
                anim_2.SetBool("Open", true);
            }
        }
        yield return new WaitForSeconds(0.5f);
        theOrder.SetTransparent("Player(Main)");
        if (door)
        {
            anim_1.SetBool("Open", false);
            if (door_count == 2)
            {
                anim_2.SetBool("Open", false);
            }
        }
        yield return new WaitForSeconds(0.5f);
        theOrder.SetUnTransparent("Player(Main)");
        thePlayer.currentMapName = transferMapName;
        theCamera.SetBound(targetBound);
        theCamera.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, theCamera.transform.position.z);
        thePlayer.transform.position = target.transform.position;
        theFade.FadeIn();
        yield return new WaitForSeconds(0.5f);
        theOrder.Move();
    }
}
