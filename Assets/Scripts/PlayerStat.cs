using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public static PlayerStat instance;
    public int character_Lv; // ����
    public int[] needExp; // �ʿ� ����ġ
    public int currentExp; // ���� ����ġ
    public int hp; // ��
    public int currentHP; // ���� ��
    public int mp; // ����
    public int currentMP; // ���� ����
    public int atk; // ���ݷ�
    public int def; // ����
    public string dmgSound;
    public GameObject prefabs_Floating_text;
    public GameObject parent;
    void Start()
    {
        instance = this;
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
            Debug.Log("ü�� 0 Game Over");
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
}
