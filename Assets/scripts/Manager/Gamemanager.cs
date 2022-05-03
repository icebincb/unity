using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Gamemanager : Singleton<Gamemanager>
{
    public CharacterStats playerstats;
    List<IEndObserve> endObserves = new List<IEndObserve>();
    private CinemachineFreeLook followCamera;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    public void RegisterPlayer(CharacterStats player)
    {
        playerstats = player;
        //Debug.Log(player);
        followCamera = FindObjectOfType<CinemachineFreeLook>();
        if (followCamera != null)
        {
            followCamera.Follow = player.transform.GetChild(2);
            followCamera.LookAt = player.transform.GetChild(2);
        }
    }
    public void addObserve(IEndObserve observe)
    {
        endObserves.Add(observe);
    }
    public void removeObserve(IEndObserve observe)
    {
        endObserves.Remove(observe);
    }
    public void NotifyObservers()
    {
        foreach (var observer in endObserves)
        {
            
            observer.EndNotify(); 
        }
    }

    public Transform GetEntrance()
    {
        foreach (var item in FindObjectsOfType<TransitionDestination>())
        {
            if(item.destinationTage== TransitionDestination.DestinationTage.ENTRE)
            {
                return item.transform;
            }
        }
        return null;
    }

} 
