using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public enum SlotType{ BAG,WEAPON,ARMOR,ACTION}
public class SlotUI : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType;
    public ItemUI itemUI;
    private IPointerEnterHandler pointerEnterHandlerImplementation;

    public void UpdateItem()
    {
        InventoryItem item = new InventoryItem();
        switch (slotType)
        {
            case SlotType.BAG:
                itemUI.Bag = InventoryManager.Instance.inventoryData; 
                //item = itemUI.Bag.items[itemUI.Index];
                break;
            case SlotType.WEAPON: 
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                //切换武器
                if (itemUI.GetItem() != null)
                {
                    Gamemanager.Instance.playerstats.ChangeWeapon(itemUI.GetItem());
                }
                else
                {
//                    Debug.Log(Gamemanager.Instance.playerstats);
                    Gamemanager.Instance.playerstats.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR: 
                itemUI.Bag = InventoryManager.Instance.equipmentData;
                break;
            case SlotType.ACTION: 
                itemUI.Bag= InventoryManager.Instance.actionData;
                //item = itemUI.Bag.items[itemUI.Index];
                break;
        }
        //itemUI.Bag = InventoryManager.Instance.inventoryData; 
        item = itemUI.Bag.items[itemUI.Index];
        itemUI.SetItemUI(item.itemData, item.amount);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount % 2 == 0)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (itemUI.GetItem()!=null &&itemUI.GetItem().itemType == ItemType.Usable&& itemUI.Bag.items[itemUI.Index].amount>0)
        {
            Gamemanager.Instance.playerstats.ApplyHP(itemUI.GetItem().useableItemDataSo.hp);
            itemUI.Bag.items[itemUI.Index].amount -= 1;
        }
        UpdateItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItem())
        {
            InventoryManager.Instance.itemTooltip.SetUPItemInfo(itemUI.GetItem());
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
