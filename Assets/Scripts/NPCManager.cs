using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCMove
{
    [Tooltip("NPCMove�� üũ�ϸ� NPC�� ������")]
    public bool NPCmove;
    public string[] direction; // npc�� ������ ���� ����
    [Range(1, 5)]
    [Tooltip("1 = õõ�� 2 = ���� õõ�� 3 = ���� 4= ������ 5= ����������")]
    public int frequency; // npc�� ������ �������� �ӵ� ����
}
public class NPCManager : MovingObject
{
    [SerializeField]
    public NPCMove npc;

    void Start()
    {
        queue = new Queue<string>();
        StartCoroutine(MoveCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        if(npc.direction.Length != 0)
        {
            for(int i = 0; i < npc.direction.Length; i++)
            {
                yield return new WaitUntil(() => queue.Count < 2);
                base.Move(npc.direction[i], npc.frequency);
                if(i == npc.direction.Length - 1)
                {
                    i = -1;
                }
            }
        }
    }
}