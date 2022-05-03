using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventory",menuName ="Inventory/InventoryData")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> items=new List<InventoryItem>();

    public void AddItem(ItemData_SO itemData_SO,int amount)
    {
        bool found=false;
        if (itemData_SO.stackable)
        {
            foreach(var item in items)
            {
                if (item.itemData == itemData_SO)
                {
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData == null && !found)
            {
                items[i].itemData = itemData_SO;
                items[i].amount = amount;
                break;
            }
        }
    }
}
[System.Serializable]
public class InventoryItem
{
    
    public ItemData_SO itemData;
    public int amount;
}