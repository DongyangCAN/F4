using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBGM : MonoBehaviour
{
    BGMManager BGM;
    public int playMusicTrack;
    void Start()
    {
        BGM = FindObjectOfType<BGMManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(abc());
        //BGM.Play(playMusicTrack);
        //this.gameObject.SetActive(false); 한번만 실행
    }
    IEnumerator abc()
    {
        BGM.FadeOutMusic();
        yield return new WaitForSeconds(3f);
        BGM.FadeInMusic();
    }
}
