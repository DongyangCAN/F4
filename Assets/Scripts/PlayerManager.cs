using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MovingObject
{
    static public PlayerManager instance;
    private AudioManager theAudio;
    private SaveEndLoad theSEL;
    public string currentMapName;
    public string currentSceneName;
    // 뛰기 제어
    public float runSpeed;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    // 중복 키 제어
    private bool canMove = true;
    private Coroutine moveCoroutine;
    // 소리 제어
    public string walkSound_1;
    public string walkSound_2;
    public string walkSound_3;
    public string walkSound_4;
    // 움직임 제어
    public bool notMove = false;
    private bool attacking = false;
    public float attackDelay;
    private float currentAttackDelay;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        queue = new Queue<string>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        theAudio = FindObjectOfType<AudioManager>();
        theSEL = FindObjectOfType<SaveEndLoad>();
    }
    IEnumerator MoveCoroutine() // 중복 키 제어
    {
        while (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0 && !notMove && !attacking)
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
            if (vector.x != 0)
            {
                vector.y = 0;
            }
            animator.SetFloat("DirX", vector.x);
            animator.SetFloat("DirY", vector.y);

            bool checkCollsionFlag = base.CheckCollsion();
            if(checkCollsionFlag)
            {
                break;
            }

            animator.SetBool("Walking", true);

            int temp = Random.Range(1, 2);
            switch (temp)
            {
                case 1:
                    theAudio.Play(walkSound_1);
                    break;
                case 2:
                    theAudio.Play(walkSound_2);
                    break;
                case 3:
                    theAudio.Play(walkSound_3);
                    break;
                case 4:
                    theAudio.Play(walkSound_4);
                    break;
            }
            boxCollider.offset = new Vector2(vector.x * 0.3f * speed * walkCount, vector.y * 0.3f * speed * walkCount);
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
                if (currentWalkCount == 25)
                {
                    boxCollider.offset = Vector2.zero;
                }
                yield return new WaitForSeconds(0.01f);
            }
            currentWalkCount = 0;
        }
        animator.SetBool("Walking", false);
        canMove = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            theSEL.CallSave();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            theSEL.CallLoad();
        }
        if (canMove && !notMove && !attacking)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                canMove = false;
                StartCoroutine(MoveCoroutine());
            }
        }
        if(!notMove && !attacking)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentAttackDelay = attackDelay;
                attacking = true;
                animator.SetBool("Attacking", true);
            }
        }
        if (attacking)
        {
            currentAttackDelay -= Time.deltaTime;
            if(currentAttackDelay <= 0)
            {
                animator.SetBool("Attacking", false);
                attacking = false;
            }
        }
    }
}
