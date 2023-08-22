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
    private SaveEndLoad SEL;
    private bool gameStart = true;
    void Start()
    {
        theFade = FindObjectOfType<FadeManager>();
        theAudio = FindObjectOfType<AudioManager>();
        thePlayer = FindObjectOfType<PlayerManager>();
        theGM = FindObjectOfType<GameManager>();
        theOrder = FindObjectOfType<OrderManager>();
        SEL = FindObjectOfType<SaveEndLoad>();
    }
    public void StartGame()
    {
        StartCoroutine(GameStartCoroutine());
    }
    IEnumerator GameStartCoroutine()
    {
        if (gameStart)
        {
            SEL.StartGamePoint();
            gameStart = false;
        }
        theFade.FadeOut();
        theAudio.Play(click_sound);
        yield return new WaitForSeconds(2f);
        SEL.ReRoadGame();
        SceneManager.LoadScene("StartMain");
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
