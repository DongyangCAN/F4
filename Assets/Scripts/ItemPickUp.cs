using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public int itemID;
    public int _count;
    public string pickUpSound;
    private bool flag = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        flag = !flag;
    }
    private void Update()
    {
        if (flag && Input.GetKeyDown(KeyCode.Z))
        {
            AudioManager.instance.Play(pickUpSound);
            Inventory.instance.GetAnItem(itemID, _count);
            Destroy(this.gameObject);
        }
    }
}
