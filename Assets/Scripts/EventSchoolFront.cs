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
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theDM = FindObjectOfType<DialogueManager>();
        theOrder = FindObjectOfType<OrderManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
    }
    void Update()
    {
        if (!flag && Input.GetKey(KeyCode.Z))
        {
            flag = true;
            StartCoroutine(EventCoroutine());
        }
    }
    IEnumerator EventCoroutine()
    {
        yield return new WaitForSeconds(1f); // ������ �� ZZ���ϰ� ����
        theOrder.PreLoadCharacter(); // ����Ʈ�� ������ �ڵ� ����
        theOrder.NotMove(); // ������ X
        theDM.ShowDialogue(dialoge_1);
        yield return new WaitUntil(() => !theDM.talking); // ��ȭ�� ���� ��
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "RIGHT");
        theOrder.Move("player", "DOWN");
        yield return new WaitUntil(() => thePlayer.queue.Count == 0); // �������� ����� ��
        theFade.Flash();
        theDM.ShowDialogue(dialoge_2);
        yield return new WaitUntil(() => !theDM.talking); 
        theOrder.Move(); // ������ O
    }
}
