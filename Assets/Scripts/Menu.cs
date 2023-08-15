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
    public string call_sound;
    public string cancel_sound;
    private bool activated;
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
