using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSchoolFront : MonoBehaviour
{
    public Dialogue dialoge_1;
    public Dialogue dialoge_2;
    private DialogueManager theDM;
    private OrderManager theOrder;
    private PlayerManager thePlayer;
    private FadeManager theFade;
    private bool flag = false;
    private bool playerOnBox = false;
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBox = true;
        }
    }

    // 플레이어가 박스에서 벗어났을 때 playerOnBox를 false로 설정
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBox = false;
        }
    }

    void Update()
    {
        // 플레이어가 박스 위에 있고, flag가 false일 때만 Z 키를 누르면 이벤트 실행
        if (playerOnBox && !flag && Input.GetKey(KeyCode.Z))
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    IEnumerator EventCoroutine()
    {
        yield return new WaitForSeconds(1f); // 시작할 때 ZZ못하게 막음
        theOrder.PreLoadCharacter(); // 리스트에 실행할 코드 넣음
        theOrder.NotMove(); // 움직임 X
        theDM.ShowDialogue(dialoge_1);
        yield return new WaitUntil(() => !theDM.talking); // 대화가 끝날 때
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "DOWN");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0); // 움직임이 종료될 때
        theFade.Flash();
        theDM.ShowDialogue(dialoge_2);
        yield return new WaitUntil(() => !theDM.talking); 
        theOrder.Move(); // 움직임 O
    }
}
