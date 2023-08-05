using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    public GameObject prefabs_Floating_text;
    public GameObject parent;
    public string atkSound;
    private PlayerStat thePlayerStat;
    void Start()
    {
        thePlayerStat = FindObjectOfType<PlayerStat>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "enemy")
        {
            int dmg = collision.gameObject.GetComponent<EnemyStat>().Hit(thePlayerStat.atk);
            AudioManager.instance.Play(atkSound);
            Vector3 vector = collision.transform.position;
            vector.y += 1;
            GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(Vector3.zero));
            clone.GetComponent<FloatingText>().text.text = dmg.ToString();
            clone.GetComponent<FloatingText>().text.color = Color.red;
            clone.GetComponent<FloatingText>().text.fontSize = 25;
            clone.transform.SetParent(parent.transform);
        }
    }
}
