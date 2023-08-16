using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private FadeManager theFade;
    private AudioManager theAudio;
    public string click_sound;
    private PlayerManager thePlayer;
    private GameManager theGM;
    public GameObject hpBar;
    public GameObject mpBar;
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
    }
    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine()
    {
        theFade.FadeOut();
        theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);
        hpBar.SetActive(true);
        mpBar.SetActive(true);
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        color.a = 1f;
        thePlayer.GetComponent<SpriteRenderer>().color = color;
        thePlayer.currentMapName = "StartPoint";
        thePlayer.currentSceneName = "StartMain";
        theGM.LoadStart();
        SceneManager.LoadScene("StartMain");
    }
    public void ExitGame()
    {
        theAudio.Play(click_sound);
        Application.Quit();
    }
}
