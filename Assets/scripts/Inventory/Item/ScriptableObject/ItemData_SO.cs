using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public  enum ItemType { Usable,Weapon,Armor}
[CreateAssetMenu(fileName = "New Data", menuName = "Inventory/ItemData")]
public class ItemData_SO : ScriptableObject
{
    [Header("Stats info")]
    public ItemType itemType;
    public string itemName;
    public Sprite itemIcon;
    public int itemAmount;
    [TextArea]
    public string discription;
    public bool stackable;
    [Header("UseableItem")] public UseableItemData_SO useableItemDataSo;
    [Header("Weapon")]
    public GameObject weaponPrefab;
    public AttackData_SO weaponData;
    public AnimatorOverrideController weaponAnimator;
}
