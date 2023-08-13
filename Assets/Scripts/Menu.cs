using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
