using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class SenceController : Singleton<SenceController>,IEndObserve
{
    public GameObject playerPrefeb;
    public SeneceFader senecefaderPrefeb;
    GameObject player;
    NavMeshAgent nav;
    bool fadeFinished;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        Gamemanager.Instance.addObserve(this);
        fadeFinished = true;
    }
    public void TransitionToDestination(TransitionPoint transitionPoint)
    {
        switch (transitionPoint.transType)
        {
            case TransitionPoint.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, transitionPoint.destinationTage));
                break;
            case TransitionPoint.TransitionType.DifferentScene:
                
                StartCoroutine(Transition(transitionPoint.scenneName, transitionPoint.destinationTage));
                
                break;
        }
    }
    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTage destinationTage)
    {
        //保存数据
        SeneceFader seneceFader = Instantiate(senecefaderPrefeb);
        yield return StartCoroutine(seneceFader.FadeOut(2f));
        Savemanager.Instance.SavePlayerData();
        InventoryManager.Instance.SaveData();
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            //yield return 是否等待这条命令执行
            yield return SceneManager.LoadSceneAsync(sceneName);
            yield return Instantiate(playerPrefeb, GetDestination(destinationTage).transform.position, GetDestination(destinationTage).transform.rotation);
            Savemanager.Instance.LoadPlayerData();
            yield return StartCoroutine(seneceFader.FadeIn(2f));
            yield break;
                 
        }
        else { 
        player = Gamemanager.Instance.playerstats.gameObject;
        nav = player.GetComponent<NavMeshAgent>();
        nav.enabled = false;
       // Destroy(player);
        player.transform.SetPositionAndRotation(GetDestination(destinationTage).transform.position, GetDestination(destinationTage).transform.rotation);
        nav.enabled = true;
            yield return StartCoroutine(seneceFader.FadeIn(2f));
            yield return null;
        }
    }
    private TransitionDestination GetDestination(TransitionDestination.DestinationTage destinationTage)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();
        for(int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTage == destinationTage)
                return entrances[i];
        }
        return null;
    }
    public void TranstoLoadMain()
    {
        StartCoroutine(LoadMain());
    }
    public void TranstoLoadGame()
    {
        StartCoroutine(LoadLevel(Savemanager.Instance.SceneName));
    }
    public void TranstoFirstLevel()
    {
        StartCoroutine(LoadLevel("Demo 1"));
    }
    IEnumerator LoadLevel(string scene)
    {
        SeneceFader seneceFader = Instantiate(senecefaderPrefeb);
        if (scene != "")
        {
            yield return StartCoroutine(seneceFader.FadeOut(2f));
            yield return SceneManager.LoadSceneAsync(scene);
            yield return player = Instantiate(playerPrefeb, Gamemanager.Instance.GetEntrance().position, Gamemanager.Instance.GetEntrance().rotation);

            //保存数据
            Savemanager.Instance.SavePlayerData();
            InventoryManager.Instance.SaveData();
            yield return StartCoroutine(seneceFader.FadeIn(2f));
            yield break;
        }
        
    }
    IEnumerator LoadMain()
    {
        SeneceFader seneceFader = Instantiate(senecefaderPrefeb);
        yield return StartCoroutine(seneceFader.FadeOut(2f));
        yield return SceneManager.LoadSceneAsync("Demo 2");
        yield return StartCoroutine(seneceFader.FadeIn(2f));
        yield break;
    }

    public void EndNotify()
    {
        if (fadeFinished)
        {
        StartCoroutine(LoadMain());
            fadeFinished = false;
        }
        
    }
}
