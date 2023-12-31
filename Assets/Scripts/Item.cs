using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public int itemID; // 아이템 고유 ID (중복X)
    public string itemName; // 아이템 이름
    public string itemDescription; // 아이템 설명
    public int itemCount; // 소지 개수
    public Sprite itemIcon;
    public ItemType itemType;
    public enum ItemType
    {
        Use,
        Equip,
        Quest,
        ETC
    }
    public int atk;
    public int def;
    public int recover_hp;
    public int recover_mp;
    public Item(int _itemID, string _itemName, string _itemDes, ItemType _itemType,
                int _atk = 0, int _def = 0, int _recover_hp = 0, int _recover_mp = 0, int _itemCount = 1)
    {
        itemID = _itemID;
        itemName = _itemName;
        itemDescription = _itemDes;
        itemType = _itemType;
        itemCount = _itemCount;
        itemIcon = Resources.Load("ItemIcon/" + _itemID.ToString(), typeof(Sprite)) as Sprite;
        atk = _atk;
        def = _def;
        recover_hp = _recover_hp;
        recover_mp = _recover_mp;
    }
}
