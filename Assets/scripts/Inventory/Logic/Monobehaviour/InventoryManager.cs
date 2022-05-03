using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InventoryManager : Singleton<InventoryManager>
{   
    public class DragData
    {
        public SlotUI originalHolder;
        public RectTransform originalParent;
    }
    //最后添加模块用于保存数据
    [Header("Inventory Data")]
    public InventoryData_SO inventoryData;
    public InventoryData_SO inventoryTempData;
    public InventoryData_SO actionData;
    public InventoryData_SO actionTempData;
    public InventoryData_SO equipmentData;
    public InventoryData_SO equipmentTempData;
    [Header("Contaniers")]
    public ContainierUI inventoryUI;
    public ContainierUI actionUI;
    public ContainierUI equipmentUI;
    [Header("DragCanvas")]
    public Canvas dargCanvas;
    public DragData currentData;

    private bool isopenBag = false;
    private bool isopenInfo = false;
    [Header("UI Pannel")] 
    public GameObject BagUI;
    public GameObject PlayerInfoUI;
    [Header("States Text")] 
    public Text HPtext;
    public Text ATKtext;
    public Text DEFtext;
    [Header("Tooltip")] public ItemTooltip itemTooltip;
    private void Start()
    {
        LoadData();
        inventoryUI.RefreshUI();
        equipmentUI.RefreshUI();
        actionUI.RefreshUI();
        BagUI.SetActive(isopenBag);
        PlayerInfoUI.SetActive(isopenInfo);
    }

    protected override void Awake()
    {
        base.Awake();
        if (inventoryTempData != null)
            inventoryData = Instantiate(inventoryTempData);
        if (actionTempData!= null)
            actionData = Instantiate(actionTempData);
        if (inventoryTempData != null)
            equipmentData = Instantiate(equipmentTempData);
    }

    private void Update(){
        UIControll();
        UpdateStatesUI((int) Gamemanager.Instance.playerstats.CurrentHealth,
            (int) Gamemanager.Instance.playerstats.CurrentDefence,
            (int) Gamemanager.Instance.playerstats.attackData_SO.maxDamage,
            (int) Gamemanager.Instance.playerstats.attackData_SO.minDamage);
    }

    public void SaveData()
    {
        Savemanager.Instance.Save(inventoryData,inventoryData.name);
        Savemanager.Instance.Save(actionData,actionData.name);
        Savemanager.Instance.Save(equipmentData,equipmentData.name);
    }

    public void LoadData()
    {
        Savemanager.Instance.Load(inventoryData,inventoryData.name);
        Savemanager.Instance.Load(actionData,actionData.name);
        Savemanager.Instance.Load(equipmentData,equipmentData.name);
    }

    public void UpdateStatesUI(int hp,int def,int maxatk,int minatk)
    {
        HPtext.text = hp.ToString();
        ATKtext.text = minatk + "-" + maxatk;
        DEFtext.text = def.ToString();
    }
    private void UIControll()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isopenBag = !isopenBag;
            BagUI.SetActive(isopenBag);
           
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            isopenInfo = !isopenInfo;
            PlayerInfoUI.SetActive(isopenInfo);
           
        }
    }
    #region 检查拖拽物品是否在每一个slot范围内

    public bool CheckIventoryUI(Vector3 position)
    {
        for (int i = 0; i < inventoryUI.slotUIs.Length; i++)
        {
            RectTransform t=inventoryUI.slotUIs[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }
    public bool CheckActionUI(Vector3 position)
    {
        for (int i = 0; i < actionUI.slotUIs.Length; i++)
        {
            RectTransform t=actionUI.slotUIs[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }
    public bool CheckWeaponUI(Vector3 position)
    {
        for (int i = 0; i < equipmentUI.slotUIs.Length; i++)
        {
            RectTransform t=equipmentUI.slotUIs[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(t, position))
            {
                return true;
            }
        }

        return false;
    }
    #endregion
}
