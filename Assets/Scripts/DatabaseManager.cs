using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    static public DatabaseManager instance;
    #region Awake
    private void Awake() // start보다 먼저 발생하는 내장함수
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }
    #endregion Awake
    public string[] var_name;
    public float[] var;
    public string[] switch_name;
    public bool[] switches;
    private void Start()
    {
        
    }
}
