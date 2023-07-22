using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestRain : MonoBehaviour
{
    private WeatherManager theWeather;
    private bool rain = true;
    void Start()
    {
        theWeather = FindObjectOfType<WeatherManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Rain());
    }
    IEnumerator Rain()
    {
        Debug.Log("1");
        if (rain)
        {
            theWeather.RainDrop();
            yield return new WaitForSeconds(2f);
            theWeather.Rain();
            rain = false;
        }
        else
        {
            theWeather.RainStop();
            rain = true;
        }
    }
}
