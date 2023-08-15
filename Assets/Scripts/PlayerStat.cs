using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;
    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public int character_Lv; // 레벨
    public int[] needExp; // 필요 경험치
    public int currentExp; // 현재 경험치
    public int hp; // 피
    public int currentHP; // 현재 피
    public int mp; // 마나
    public int currentMP; // 현재 마나
    public int atk; // 공격력
    public int def; // 방어력
    public int recover_hp;
    public int recover_mp;
    public string dmgSound;
    public float time;
    private float current_time;
    public GameObject prefabs_Floating_text;
    public GameObject parent;
    public Slider hpSlider;
    public Slider mpSlider;
    void Start()
    {
        instance = this;
        current_time = time;
    }
    public void Hit(int _enemyAtk)
    {
        int dmg;
        if(def >= _enemyAtk)
        {
            dmg = 1;
        }
        else
        {
            dmg = _enemyAtk - def;
        }
        currentHP -= dmg;
        if(currentHP <= 0)
        {
            Debug.Log("체력 0 Game Over");
        }
        AudioManager.instance.Play(dmgSound);
        Vector3 vector = this.transform.position;
        vector.y += 1;
        GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(Vector3.zero));
        clone.GetComponent<FloatingText>().text.text = dmg.ToString();
        clone.GetComponent<FloatingText>().text.color = Color.red;
        clone.GetComponent<FloatingText>().text.fontSize = 25;
        clone.transform.SetParent(parent.transform);
        StopAllCoroutines();
        StartCoroutine(HitCoroutine());
    }
    IEnumerator HitCoroutine()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
        color.a = 0f;
        GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        GetComponent<SpriteRenderer>().color = color;
    }
    void Update()
    {
        hpSlider.maxValue = hp;
        mpSlider.maxValue = mp;
        hpSlider.value = currentHP;
        mpSlider.value = currentMP;
        if (currentExp >= needExp[character_Lv])
        {
            character_Lv++;
            hp += character_Lv * 2;
            mp = character_Lv + 2;
            currentHP = hp;
            currentMP = mp;
            atk++;
            def++;
            currentExp = 0;
        }   
        current_time -= Time.deltaTime;
        if(current_time <= 0)
        {
            if(recover_hp > 0)
            {
                if(currentHP + recover_hp <= hp)
                {
                    currentHP += recover_hp;
                }
                else
                {
                    currentHP = hp;
                }
                current_time = time;
            }
        }
    }
}
