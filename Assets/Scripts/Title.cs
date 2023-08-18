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
    private OrderManager theOrder;
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
        theOrder = FindObjectOfType<OrderManager>();
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
        Color color = thePlayer.GetComponent<SpriteRenderer>().color;
        theOrder.Move();
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
