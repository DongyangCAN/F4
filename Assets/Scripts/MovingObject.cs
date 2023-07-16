using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed;
    protected Vector3 vector; // protected = 부모 자식 O, 외부 X
    public int walkCount;
    protected int currentWalkCount;
    public BoxCollider2D boxCollider;
    public LayerMask layerMask;
    public Animator animator;
    protected bool npcCanMove = true;
    protected void Move(string _dir, int _frequency)
    {
        StartCoroutine(MoveCoroutine(_dir, _frequency));
    }
    IEnumerator MoveCoroutine(string _dir, int _frequency)
    {
        npcCanMove = false;
        vector.Set(0, 0, vector.z);
        switch (_dir)
        {
            case "UP":
                vector.y = 1f;
                break;
            case "DOWN":
                vector.y = -1f;
                break;
            case "RIGHT":
                vector.x = 1f;
                break;
            case "LEFT":
                vector.x = -1f;
                break;
        }
        animator.SetFloat("DirX", vector.x);
        animator.SetFloat("DirY", vector.y);
        animator.SetBool("Walking", true);
        while (currentWalkCount < walkCount)
        {
            transform.Translate(vector.x * speed, vector.y * speed, 0);
            currentWalkCount++;
            yield return new WaitForSeconds(0.01f);
        }
        currentWalkCount = 0;
        if (_frequency != 5)
        {
            animator.SetBool("Walking", false);
        }
        npcCanMove = true;
    }
    protected bool CheckCollsion()
    {
        RaycastHit2D hit; // A지점에서 B지점까지 레이저를 쏘는데 도달하면 NULL 장애물이 있으면 장애물을 리턴
        Vector2 start = transform.position; // A지점 캐릭터의 현재 위치값
        Vector2 end = start + new Vector2(vector.x * speed * walkCount, vector.y * speed * walkCount); // B지점 캐릭터가 이동하고자 하는 위치 값
        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, layerMask);
        boxCollider.enabled = true;

        if (hit.transform != null)
        {
            return true;
        }
        return false;
    }
}
