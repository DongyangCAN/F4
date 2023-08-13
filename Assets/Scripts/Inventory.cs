using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public string key_sound;
    public string enter_sound;
    public string cancel_sound;
    public string open_sound;
    public string beep_sound;
    public Text Description_Text; // �ο� ����
    public string[] tabDescription; // �� �ο� ����
    public Transform tf; // slot �θ�ü
    public GameObject go; // �κ��丮 �Ҽ�ȭ ��Ȱ��ȭ
    public GameObject[] selectedTabImages;
    public GameObject go_OOC; // ���ý� Ȱ��ȭ ��Ȱ��ȭ
    public GameObject pretab_floating_text;
    private int selectedItem; // ���õ� ������
    private int selectedTab; // ���õ� ��

    private int page; // ������
    private int slotCount; // Ȱ��ȭ�� ������ ����
    private const int MAX_SLOTS_COUNT = 8; // �ִ� ���� ����

    private bool activated; // �κ��丮 Ȱ���� �� true;
    private bool tabActivated; // �� Ȱ��ȭ �� true
    private bool itemActivated; // ������ Ȱ��ȭ �� true
    private bool stopKeyInput; // Ű �Է� ����
    private bool preventExec;  // �ߺ����� ����
    private OOC theOOC;
    private DatabaseManager theDatabase;
    private AudioManager theAudio;
    private OrderManager theOrder;
    private Equipment theEquip;
    private InventorySlot[] slots; // �κ��丮 ���Ե�
    private List<Item> inventoryItemList; // �÷��̾ ������ ������ ����Ʈ
    private List<Item> inventoryTabList; // ������ �ۿ� ���� �ٸ��� ������ ������ ����Ʈ
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);
    void Start()
    {
        instance = this;
        theDatabase = FindObjectOfType<DatabaseManager>();  
        theAudio = FindObjectOfType<AudioManager>();
        theOrder = FindObjectOfType<OrderManager>();
        theOOC = FindObjectOfType<OOC>();
        theEquip = FindObjectOfType<Equipment>();
        inventoryItemList = new List<Item>();
        inventoryTabList = new List<Item>();
        slots = tf.GetComponentsInChildren<InventorySlot>();
    }
    public List<Item> SaveItem()
    {
        return inventoryItemList;
    }
    public void LoadItem(List<Item> _itemList)
    {
        inventoryItemList = _itemList;
    }
    public void EquipToInventory(Item _item)
    {
        inventoryItemList.Add(_item);
    }
    public void GetAnItem(int _itemID, int _count = 1) 
    {
        for(int i = 0; i < theDatabase.itemList.Count; i++) // �����ͺ��̽� ������ �˻�
        {
            if(_itemID == theDatabase.itemList[i].itemID) // �������� ã�Ƽ� �߰�
            {
                var clone = Instantiate(pretab_floating_text, PlayerManager.instance.transform.position, Quaternion.Euler(Vector3.zero));
                clone.GetComponent<FloatingText>().text.text = theDatabase.itemList[i].itemName + " " + _count + "�� ȹ��";
                clone.transform.SetParent(this.transform);
                for(int j = 0; j < inventoryItemList.Count; j++) // �������� �̹� ������ 1 ������
                {
                    if (inventoryItemList[j].itemID == _itemID)
                    {
                        if (inventoryItemList[j].itemType == Item.ItemType.Use)
                        {
                            inventoryItemList[j].itemCount += _count;
                        }
                        else
                        {
                            inventoryItemList.Add(theDatabase.itemList[i]);
                        }
                        return;
                    }
                }
                inventoryItemList.Add(theDatabase.itemList[i]);
                inventoryItemList[inventoryItemList.Count - 1].itemCount = _count;
                return;
            }
        }
        Debug.LogError("�����ͺ��̽��� �ش� ID���� ���� �������� �������� �ʽ��ϴ�.");
    }
    public void ShowTab()
    {
        RemoveSlot();
        SelectedTab();
    }
    public void RemoveSlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].RemoveItem();
            slots[i].gameObject.SetActive(false);
        }
    }
    public void SelectedTab()
    {
        StopAllCoroutines();
        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
        color.a = 0f;
        for (int i = 0; i < selectedTabImages.Length; i++)
        {
            selectedTabImages[i].GetComponent<Image>().color = color;
        }
        Description_Text.text = tabDescription[selectedTab];
        StartCoroutine(SelectedTabEffectCoroutine());
    }
    IEnumerator SelectedTabEffectCoroutine()
    {
        while (tabActivated)
        {
            Color color = selectedTabImages[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                yield return waitTime;
            }
        }
    }
    public void ShowItem()
    {
        inventoryTabList.Clear();
        RemoveSlot();
        selectedItem = 0;
        page = 0;
        switch (selectedTab)
        {
            case 0:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Use == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
            case 1:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Equip == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.Quest == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
            case 3:
                for (int i = 0; i < inventoryItemList.Count; i++)
                {
                    if (Item.ItemType.ETC == inventoryItemList[i].itemType)
                    {
                        inventoryTabList.Add(inventoryItemList[i]);
                    }
                }
                break;
        }
        ShowPage();
        SelectedItem();
    }
    public void ShowPage()
    {
        slotCount = -1;
        for (int i = page * MAX_SLOTS_COUNT; i < inventoryTabList.Count; i++)
        {
            slotCount = i - (page * MAX_SLOTS_COUNT);
            slots[slotCount].gameObject.SetActive(true);
            slots[slotCount].AddItem(inventoryTabList[i]);
            if(slotCount == MAX_SLOTS_COUNT - 1)
            {
                break;
            }
        }
    }
    public void SelectedItem()
    {
        StopAllCoroutines();
        if (slotCount > -1)
        {
            selectedItem = Mathf.Clamp(selectedItem, 0, inventoryTabList.Count - 1); // ���� ������ �ʰ����� ���ϰ� ����
            Color color = slots[0].selected_Item.GetComponent<Image>().color;
            color.a = 0f;
            for (int i = 0; i <= slotCount; i++)
            {
                slots[i].selected_Item.GetComponent<Image>().color = color;
            }
            Description_Text.text = inventoryTabList[selectedItem].itemDescription;
            StartCoroutine(SelectedItemEffectCoroutine());
        }
        else
        {
            Description_Text.text = "�ش� Ÿ���� �������� �����ϰ� ���� �ʽ��ϴ�.";
        }
    }
    IEnumerator SelectedItemEffectCoroutine()
    {
        while (itemActivated)
        {
            Color color = slots[0].GetComponent<Image>().color;
            while (color.a < 0.5f)
            {
                color.a += 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
            while (color.a > 0f)
            {
                color.a -= 0.03f;
                slots[selectedItem].selected_Item.GetComponent<Image>().color = color;
                yield return waitTime;
            }
        }
    }
    void Update()
    {
        if (!stopKeyInput)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                activated = !activated;
                if (activated)
                {
                    theAudio.Play(open_sound);
                    theOrder.NotMove();
                    go.SetActive(true);
                    selectedTab = 0;
                    tabActivated = true;
                    itemActivated = true;
                    ShowTab();
                }
                else
                {
                    theAudio.Play(cancel_sound);
                    StopAllCoroutines();
                    go.SetActive(false);
                    tabActivated = false;
                    itemActivated = false;
                    theOrder.Move();
                }
            }
            if (activated)
            {
                if (tabActivated)
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        if (selectedTab < selectedTabImages.Length - 1)
                        {
                            selectedTab++;
                        }
                        else
                        {
                            selectedTab = 0;
                        }
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        if (selectedTab > 0)
                        {
                            selectedTab--;
                        }
                        else
                        {
                            selectedTab = selectedTabImages.Length - 1;
                        }
                        theAudio.Play(key_sound);
                        SelectedTab();
                    }
                    else if (Input.GetKeyDown(KeyCode.Z))
                    {
                        theAudio.Play(enter_sound);
                        Color color = selectedTabImages[selectedTab].GetComponent<Image>().color;
                        color.a = 0.25f;
                        selectedTabImages[selectedTab].GetComponent<Image>().color = color;
                        itemActivated = true;
                        tabActivated = false;
                        preventExec = true;
                        ShowItem();
                    }
                }
                else if (itemActivated)
                {
                    if (inventoryTabList.Count > 0)
                    {
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            if(selectedItem + 2 > slotCount)
                            {
                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT) 
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -2;
                            }
                            if (selectedItem < slotCount)
                            {
                                selectedItem += 2;
                            }
                            else
                            {
                                selectedItem %= 2;
                            }
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            if (selectedItem - 2 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }
                            if (selectedItem > 1)
                            {
                                selectedItem -= 2;
                            }
                            else
                            {
                                selectedItem = slotCount - selectedItem;
                            }
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            if (selectedItem + 1 > slotCount)
                            {
                                if (page < (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT)
                                    page++;
                                else
                                    page = 0;
                                RemoveSlot();
                                ShowPage();
                                selectedItem = -1;
                            }
                            if (selectedItem < slotCount)
                            {
                                selectedItem++;
                            }
                            else
                            {
                                selectedItem = 0;
                            }
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            if (selectedItem - 1 < 0)
                            {
                                if (page != 0)
                                    page--;
                                else
                                    page = (inventoryTabList.Count - 1) / MAX_SLOTS_COUNT;
                                RemoveSlot();
                                ShowPage();
                            }
                            if (selectedItem > 0)
                            {
                                selectedItem--;
                            }
                            else
                            {
                                selectedItem = slotCount;
                            }
                            theAudio.Play(key_sound);
                            SelectedItem();
                        }
                        else if (Input.GetKeyDown(KeyCode.Z) && !preventExec)
                        {
                            if (selectedTab == 0)
                            {
                                StartCoroutine(OOCCoroutine("���", "���"));
                            }
                            else if (selectedTab == 1)
                            {
                                StartCoroutine(OOCCoroutine("����", "���"));
                            }
                            else
                            {
                                theAudio.Play(beep_sound);
                            }
                        }
                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        theAudio.Play(cancel_sound);
                        StopAllCoroutines();
                        itemActivated = false;
                        tabActivated = true;
                        ShowTab();
                    }
                }
                if (Input.GetKeyUp(KeyCode.Z))
                {
                    preventExec = false;
                }
            }
        }
    }
    IEnumerator OOCCoroutine(string _up, string _down)
    {
        theAudio.Play(enter_sound);
        stopKeyInput = true;
        go_OOC.SetActive(true);
        theOOC.ShowTwoChoice(_up, _down);
        yield return new WaitUntil(() => !theOOC.activated);
        if (theOOC.GetResult())
        {
            for(int i = 0; i < inventoryItemList.Count; i++)
            {
                if (inventoryItemList[i].itemID == inventoryTabList[selectedItem].itemID)
                {
                    if (selectedTab == 0)
                    {
                        theDatabase.UseItem(inventoryItemList[i].itemID);
                        if (inventoryItemList[i].itemCount > 1)
                        {
                            inventoryItemList[i].itemCount--;
                        }
                        else
                        {
                            inventoryItemList.RemoveAt(i);
                        }
                        ShowItem();
                        break;
                    }
                    else if(selectedTab == 1)
                    {
                        theEquip.EquipItem(inventoryItemList[i]);
                        inventoryItemList.RemoveAt(i);
                        ShowItem();
                        break;
                    }
                }
            }
        }
        stopKeyInput = false;
        go_OOC.SetActive(false);
    }
}
