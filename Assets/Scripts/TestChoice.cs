using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    private OrderManager theOrder;
    private ChoiceManager theChoice;
    [SerializeField]
    public Choice choice;
    public bool flag;
    void Start()
    {
        theOrder = FindObjectOfType<OrderManager>();
        theChoice = FindObjectOfType<ChoiceManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!flag)
        {
            StartCoroutine(ACoroutine());
        }
    }
    IEnumerator ACoroutine()
    {
        flag = true;
        theOrder.NotMove();
        theChoice.ShowChoice(choice);
        yield return new WaitUntil(()=>!theChoice.choiceIng);
        theOrder.Move();
    }
}
