using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class SlimeController : MovingObject
{
    public float attackDelay; // 공격 속도
    public float inter_MoveWaitTime; // 대기 시간
    public string atkSound;
    private float current_interMWT;
    private Vector2 playerPos; // 플레이어 좌표 값
    private int random_int;
    private string direction;
    public GameObject healthBar;
    void Start()
    {
        queue = new Queue<string>();
        current_interMWT = inter_MoveWaitTime;
    }
    void Update()
    {
        current_interMWT -= Time.deltaTime;
        if(current_interMWT <= 0)
        {
            current_interMWT = inter_MoveWaitTime;
            if (NearPlayer())
            {
                Flip();
                return;
            }
            RandomDirection();
            if (base.CheckCollsion())
            {
                return;
            }
            base.Move(direction);
            StartCoroutine(Delay());
        }
    }
    private void Flip()
    {
        Vector3 flip = transform.localScale;
        if(playerPos.x > this.transform.position.x)
        {
            flip.x = -3f;
        }
        else
        {
            flip.x = 3f;
        }
        this.transform.localScale = flip;
        healthBar.transform.localScale = flip;
        animator.SetTrigger("Attack");
        StartCoroutine(WaitCoroutine());
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(attackDelay);
        AudioManager.instance.Play(atkSound);
        if (NearPlayer())
        {
            PlayerStat.instance.Hit(GetComponent<EnemyStat>().atk);
        }
    }
    private bool NearPlayer()
    {
        playerPos = PlayerManager.instance.transform.position;
        if(Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 1f)
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }
        if (Mathf.Abs(Mathf.Abs(playerPos.y) - Mathf.Abs(this.transform.position.y)) <= speed * walkCount * 1f)
        {
            if (Mathf.Abs(Mathf.Abs(playerPos.x) - Mathf.Abs(this.transform.position.x)) <= speed * walkCount * 0.5f)
            {
                return true;
            }
        }
        return false;
    }
    private void RandomDirection()
    {
        vector.Set(0, 0, vector.z);
        random_int = Random.Range(0, 4);
        switch (random_int)
        {
            case 0:
                vector.y = 1f;
                direction = "UP";
                break;
            case 1:
                vector.y = -1f;
                direction = "DOWN";
                break;
            case 2:
                vector.x = 1f;
                direction = "RIGHT";
                break;
            case 3:
                vector.x = -1f;
                direction = "LEFT";
                break;
        }
    }
    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        boxCollider.offset = Vector2.zero;
    }
}
