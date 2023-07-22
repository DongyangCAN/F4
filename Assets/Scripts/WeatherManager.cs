using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    static public WeatherManager instance;
    #region Awake
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
    #endregion Awake
    private AudioManager theAudio;
    public ParticleSystem rain;
    public string rainSound;
    void Start()
    {
        theAudio = FindObjectOfType<AudioManager>();
    }
    public void Rain()
    {
        rain.Play();
        theAudio.Play(rainSound);
    }
    public void RainStop()
    {
        rain.Stop();
        theAudio.Stop(rainSound);
    }
    public void RainDrop()
    {
        StartCoroutine(RainDropCoroutine());
    }
    IEnumerator RainDropCoroutine()
    {
        int i = 0;
        while(i < 100)
        {
            rain.Emit(1); // 물량 조절
            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
