using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;
    private DialogueManager theDM;
    private bool eventCheck = false;
    private bool flag;
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && flag == false)
        {
            eventCheck = true;
            flag = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && eventCheck == true)
        {
            theDM.ShowDialogue(dialogue);
            eventCheck = false;
        }
    }
}
