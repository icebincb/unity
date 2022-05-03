using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode key;
    private SlotUI currentslotUI;

    private void Awake()
    {
        currentslotUI = GetComponent<SlotUI>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(key)&& currentslotUI.itemUI.GetItem())
        {
            currentslotUI.UseItem();
        }
    }
}
