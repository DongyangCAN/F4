using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class Menu : MonoBehaviour
{
    public static Menu instance;
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
    public GameObject go;
    public AudioManager theAudio;
    public OrderManager theOrder;
    public GameObject[] gos;
    private PlayerManager thePlayer;
    public string call_sound;
    public string cancel_sound;
    public GameObject hpBar;
    public GameObject mpBar;
    private bool activated;
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerManager>();    
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Continue()
    {
        activated = false;
        go.SetActive(false);
        theAudio.Play(cancel_sound);
        theOrder.Move();
    }
    public void GoToTitle()
    {
        thePlayer.currentSceneName = "Title";
        thePlayer.currentMapName = "Title";
        hpBar.SetActive(false);
        mpBar.SetActive(false);
        for(int i = 0; i < gos.Length; i++)
        {
            Destroy(gos[i]);
        }
        go.SetActive(false);
        activated = false;
        SceneManager.LoadScene("Title");
    }    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            activated = !activated;
            if (activated)
            {
                theOrder.NotMove();
                go.SetActive(true);
                theAudio.Play(call_sound);
            }
            else
            {
                go.SetActive(false);
                theAudio.Play(cancel_sound);
                theOrder.Move();
            }
        }
    }
}
