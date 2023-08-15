using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Linq;

public class SaveEndLoad : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        // 좌표
        public float playerX;
        public float playerY;
        public float playerZ;
        // 플레이어 현재 상태
        public int playerLv;
        public int playerHP;
        public int playerMP;
        public int playerCurrentHP;
        public int playerCurrentMP;
        public int playerCurrentEXP;
        public int playerHPR;
        public int playerMPR;
        // 스탯
        public int playerATK;
        public int playerDEF;
        public int added_atk;
        public int added_def;
        public int added_hpr;
        public int added_mpr;
        // 아이템
        public List<int> playerItemInventory;
        public List<int> playerItemInventoryCount;
        public List<int> playerEquipItem;
        // 장소
        public string mapName;
        public string sceneName;
        // 게임 저장
        public List<bool> swList;
        public List<string> swNameList;
        public List<string> varNameList;
        public List<float> varNumberList;
    }
    private PlayerManager thePlayer;
    private PlayerStat thePlayerStat;
    private DatabaseManager theDatabase;
    private FadeManager theFade;
    private Equipment theEquip;
    private Inventory theInven;
    public Data data;
    private Vector3 vector;
    public void CallSave()
    {
        theDatabase = FindObjectOfType<DatabaseManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        thePlayerStat = FindObjectOfType<PlayerStat>();
        theEquip = FindObjectOfType<Equipment>();
        theInven = FindObjectOfType<Inventory>();
        theFade = FindObjectOfType<FadeManager>();
        data.playerX = thePlayer.transform.position.x;
        data.playerY = thePlayer.transform.position.y;
        data.playerZ = thePlayer.transform.position.z;
        data.playerLv = thePlayerStat.character_Lv;
        data.playerHP = thePlayerStat.hp;
        data.playerMP = thePlayerStat.mp;
        data.playerCurrentHP = thePlayerStat.currentHP;
        data.playerCurrentMP = thePlayerStat.currentMP;
        data.playerCurrentEXP = thePlayerStat.currentExp;
        data.playerHPR = thePlayerStat.recover_hp;
        data.playerMPR = thePlayerStat.recover_mp;
        data.playerATK = thePlayerStat.atk;
        data.playerDEF = thePlayerStat.def;
        data.added_atk = theEquip.added_atk;
        data.added_def = theEquip.added_def;
        data.added_hpr = theEquip.added_hpr;
        data.added_mpr = theEquip.added_mpr;
        data.mapName = thePlayer.currentMapName;
        data.sceneName = thePlayer.currentSceneName;
        data.playerItemInventory.Clear();
        data.playerItemInventoryCount.Clear();
        data.playerEquipItem.Clear();
        for(int i = 0; i < theDatabase.var_name.Length; i++)
        {
            data.varNameList.Add(theDatabase.var_name[i]);
            data.varNumberList.Add(theDatabase.var[i]);
        }
        for (int i = 0; i < theDatabase.switch_name.Length; i++)
        {
            data.swNameList.Add(theDatabase.switch_name[i]);
            data.swList.Add(theDatabase.switches[i]);
        }
        List<Item> itemList = theInven.SaveItem();
        for(int i = 0; i < itemList.Count; i++)
        {
            data.playerItemInventory.Add(itemList[i].itemID);
            data.playerItemInventoryCount.Add(itemList[i].itemCount);
        }
        for(int i = 0; i < theEquip.equipItemList.Length; i++) 
        {
            data.playerEquipItem.Add(theEquip.equipItemList[i].itemID);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");
        bf.Serialize(file, data);
        file.Close();
    }
    public void CallLoad()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);
        if(file != null && file.Length > 0)
        {
            data = (Data)bf.Deserialize(file);
            theDatabase = FindObjectOfType<DatabaseManager>();
            thePlayer = FindObjectOfType<PlayerManager>();
            thePlayerStat = FindObjectOfType<PlayerStat>();
            theEquip = FindObjectOfType<Equipment>();
            theInven = FindObjectOfType<Inventory>();
            theFade.FadeOut();
            thePlayer.currentMapName = data.mapName;
            vector.Set(data.playerX, data.playerY, data.playerZ);
            thePlayer.transform.position = vector;
            thePlayerStat.character_Lv = data.playerLv;
            thePlayerStat.hp = data.playerHP;
            thePlayerStat.mp = data.playerMP;
            thePlayerStat.currentHP = data.playerCurrentHP;
            thePlayerStat.currentMP = data.playerCurrentMP;
            thePlayerStat.currentExp = data.playerCurrentEXP;
            thePlayerStat.atk = data.playerATK;
            thePlayerStat.def = data.playerDEF;
            thePlayerStat.recover_hp = data.playerHPR;
            thePlayerStat.recover_mp = data.playerMPR;
            theEquip.added_atk = data.added_atk;
            theEquip.added_def = data.added_def;
            theEquip.added_hpr = data.added_hpr;
            theEquip.added_mpr = data.added_mpr;
            theDatabase.var = data.varNumberList.ToArray();
            theDatabase.var_name = data.varNameList.ToArray();
            theDatabase.switches = data.swList.ToArray();
            theDatabase.switch_name = data.swNameList.ToArray();
            for(int i = 0; i < theEquip.equipItemList.Length; i++)
            {
                for(int j = 0; j < theDatabase.itemList.Count; j++)
                {
                    if (data.playerEquipItem[i] == theDatabase.itemList[j].itemID)
                    {
                        theEquip.equipItemList[i] = theDatabase.itemList[j];
                        break;
                    }
                }
            }
            List<Item> itemList = new List<Item>();
            for (int i = 0; i < data.playerItemInventory.Count; i++)
            {
                for (int j = 0; j < theDatabase.itemList.Count; j++)
                {
                    if (data.playerItemInventory[i] == theDatabase.itemList[j].itemID)
                    {
                        itemList.Add(theDatabase.itemList[j]);
                        break;
                    }
                }
            }
            for(int i = 0; i < data.playerItemInventoryCount.Count; i++)
            {
                itemList[i].itemCount = data.playerItemInventoryCount[i];
            }
            theInven.LoadItem(itemList);
            theEquip.ShowText();
            theEquip.ShowEquip();
            StartCoroutine(WaitCoroutine());
        }
        else
        {
            Debug.Log("저장된 세이브 파일이 없습니다.");
        }
        file.Close();
    }
    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(2);
        GameManager theGM = FindObjectOfType<GameManager>();
        theGM.LoadStart();
        if (thePlayer.currentSceneName != data.sceneName)
        {
            thePlayer.currentSceneName = data.sceneName;
            SceneManager.LoadScene(data.sceneName);
        }
    }
}
