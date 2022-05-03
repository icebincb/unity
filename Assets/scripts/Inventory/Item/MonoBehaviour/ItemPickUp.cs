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
            //����Ʒ��ӵ�����
            InventoryManager.Instance.inventoryData.AddItem(itemData, itemData.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            //װ������
            //Debug.Log("item");
            // Gamemanager.Instance.playerstats.EquipWeapon(itemData);
            Destroy(gameObject);
        }
    }
}
