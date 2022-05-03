using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    ItemUI itemUI;
    SlotUI currentHolder;
    SlotUI targetHolder;
    private void Awake()
    {
        itemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotUI>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentData = new InventoryManager.DragData();
        InventoryManager.Instance.currentData.originalHolder = GetComponentInParent<SlotUI>();
        InventoryManager.Instance.currentData.originalParent = (RectTransform)transform.parent;
        //记录原始信息
        transform.SetParent(InventoryManager.Instance.dargCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //跟随鼠标位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //放下物品，交换数据
        //是否指向UI物体
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckIventoryUI(eventData.position)||InventoryManager.Instance.CheckActionUI(eventData.position)||InventoryManager.Instance.CheckWeaponUI(eventData.position)) ;
            {
                if (eventData.pointerEnter.gameObject.GetComponent<SlotUI>())
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotUI>();
                }
                else targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotUI>();
                if(targetHolder!=InventoryManager.Instance.currentData.originalHolder)
                switch (targetHolder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.ARMOR:
                        if(currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index].itemData.itemType==ItemType.Armor)
                        SwapItem();
                        break;
                    case SlotType.ACTION:
                        if(currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index].itemData.itemType==ItemType.Usable)
                        SwapItem();
                        break;
                    case SlotType.WEAPON:
                        if(currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index].itemData.itemType==ItemType.Weapon)
                        SwapItem();
                        break;
                }
                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }
        }
        transform.SetParent(InventoryManager.Instance.currentData.originalParent);
        RectTransform t=transform as RectTransform;
        t.offsetMax =Vector2.one*0; 
        t.offsetMin =Vector2.one*0;
    }

    void SwapItem()
    {
        var targetItem = targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index];
        var tempItem = currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index];
        bool isSameItem = targetItem.itemData == tempItem.itemData;

        if (isSameItem && targetItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0;
        }
        else
        {
            currentHolder.itemUI.Bag.items[currentHolder.itemUI.Index] = targetItem;
            targetHolder.itemUI.Bag.items[targetHolder.itemUI.Index] = tempItem;
        }
    }
}
