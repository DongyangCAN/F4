using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteDia : MonoBehaviour
{
    [SerializeField]
    public Dialogue dialogue;
    private DialogueManager theDM;
    private bool eventCheck = false;
    void Start()
    {
        theDM = FindObjectOfType<DialogueManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            eventCheck = true;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && eventCheck == true)
        {
            theDM.RandomShowDialogue(dialogue);
            eventCheck = false;
        }
    }
}
