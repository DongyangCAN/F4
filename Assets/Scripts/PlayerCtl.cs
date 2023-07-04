using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    // 박스 제어
    private BoxCollider2D boxCollider;
    public LayerMask layerMask; // <- 충돌할 때 어느 레이아웃과 충돌했나?

    // 걷기 제어
    public float speed;
    private Vector3 vector;

    // 뛰기 제어
    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;

    // 그리드 제어
    public int walkCount;
    private int currentWalkCount;

    // 중복 키 제어
    private bool canMove = true;

    // 애니메이터 제어
    private Animator animator;

    // Start is called before the first frame update
    void Start() 
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    IEnumerator MoveCoroutine() // 중복 키 제어
    {
        while(Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
                applyRunFlag = true;
            }
            else
            {
                applyRunSpeed = 0;
                applyRunFlag = false;
            }
            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);
            // 애니메이션 설정
            if(vector.x != 0)
            {
                vector.y = 0;
            }
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            RaycastHit2D hit; // A지점에서 B지점까지 레이저를 쏘는데 도달하면 NULL 장애물이 있으면 장애물을 리턴
            Vector2 start = transform.position; // A지점 캐릭터의 현재 위치값
            Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); // B지점 캐릭터가 이동하고자 하는 위치 값
            boxCollider.enabled = false;
            hit = Physics2D.Linecast(start, end, layerMask);
            boxCollider.enabled = true;

            if(hit.transform != null)
            {
                break;
            }

            animator.SetBool("Walking", true);

            while (currentWalkCount < walkCount)
            {
                if (vector.x != 0)
                {
                    transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
                }
                else if (vector.y != 0)
                {
                    transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
                }
                if (applyRunFlag)
                {
                    currentWalkCount++;
                }
                currentWalkCount++;
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }  
    }
}
