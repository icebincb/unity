using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
     void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //将物品添加到背包
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //装备武器
            //Debug.Log("item");
            // Gamemanager.Instance.playerstats.EquipWeapon(itemData);
            Destroy(gameObject);
        }
    }
}
