using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPoint : MonoBehaviour
{
    public enum TransitionType
    {
        SameScene,DifferentScene
    }
    
    [Header("Transition Info")]
    public string scenneName;
    public TransitionType transType;
    public TransitionDestination.DestinationTage destinationTage;

    private bool canTrans;

    private void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.F) && canTrans)
        {
            //SceneController ´«ËÍ
            SenceController.Instance.TransitionToDestination(this);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canTrans = false ;
        }
    }
}
