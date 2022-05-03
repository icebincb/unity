using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainierUI : MonoBehaviour
{
    public SlotUI[] slotUIs;

    public void RefreshUI()
    {
        //Debug.Log(slotUIs.Length);
        for(int i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].itemUI.Index = i;
            //Debug.Log(i);
            slotUIs[i].UpdateItem();
        }
    }
}
