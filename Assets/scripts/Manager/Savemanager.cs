using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Savemanager : Singleton<Savemanager>
{
    string sceneName="";
    public string SceneName { get { return PlayerPrefs.GetString(sceneName); } }
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SenceController.Instance.TranstoLoadMain();
        }
            if (Input.GetKeyDown(KeyCode.S))
        {
            SavePlayerData();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            LoadPlayerData();
        }
    }
    public void SavePlayerData()
    {
        //Debug.Log("save");
        Save(Gamemanager.Instance.playerstats.charactorData, Gamemanager.Instance.playerstats.charactorData.name);
    }
    public void LoadPlayerData()
    {
        //Debug.Log("Load");
        Load(Gamemanager.Instance.playerstats.charactorData, Gamemanager.Instance.playerstats.charactorData.name);
    }
    public void Save(object data,string key)
    {//保存数据
        var jsonData = JsonUtility.ToJson(data,true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.SetString(sceneName, SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
    public void Load(object data,string key)
    {//拿出数据
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
