using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public event Action<float, float> UpdateHealthBarOnAttack;
    public CharacterData_SO templateData;
    public CharacterData_SO charactorData;
    public AttackData_SO attackData_SO;
    private AttackData_SO baseattackData_SO;
    private RuntimeAnimatorController baseAnimator;
    [Header("Weapon")]
    public Transform weaponSlot;
    [HideInInspector]
    public bool isCritical;
    #region data from data_so
    public float MaxHealth
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.maxHP;
            }
            return 0;
        }
        set
        {
            charactorData.maxHP =value;

        }
    }
    public float CurrentHealth
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.currentHP;
            }
            return 0;
        }
        set
        {
            charactorData.currentHP = value;

        }
    }

    public float BaseDefence
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.base_defence;
            }
            return 0;
        }
        set
        {
            charactorData.base_defence = value;

        }
    }
    public float CurrentDefence
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.current_defence;
            }
            return 0;
        }
        set
        {
            charactorData.current_defence = value;

        }
    }
    public float Max_defence
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.max_defence;
            }
            return 0;
        }
        set
        {
            charactorData.max_defence = value;

        }
    }
    public int Base_EXP
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.baseExp;
            }
            return 0;
        }
        set
        {
            charactorData.baseExp = value;

        }
    }
    public int Current_EXP
    {
        get
        {
            if (charactorData != null)
            {
                return charactorData.currentExp;
            }
            return 0;
        }
        set
        {
            charactorData.currentExp = value;

        }
    }
    #endregion

    private void Awake()
    {
        if (templateData != null) 
        { 
        charactorData = Instantiate(templateData);
            //Debug.Log("fuzhi");
        }

        baseattackData_SO = Instantiate(attackData_SO);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
        

    }
    #region character combat
    public void TakeDamage(CharacterStats attacker,CharacterStats defener)
    {
       
        float damage = attacker.CurrentDamage() *  (1-defener.CurrentDefence / 100);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (isCritical)
        {
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        //Debug.Log(damage);

        //update UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //update 经验
        if (CurrentHealth <= 0)
        {
            attacker.charactorData.UpdateExp(charactorData.killPoint);
        }
    }
    public void TakeDamage(float damage, CharacterStats defener)
    {
        float currentdamage = damage * (1 - defener.CurrentDefence / 100);
        CurrentHealth = Mathf.Max(CurrentHealth - currentdamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        if (CurrentHealth <= 0)
            Gamemanager.Instance.playerstats.charactorData.UpdateExp(charactorData.killPoint);
    }
    private float CurrentDamage()
    {
        float coredamage = UnityEngine.Random.Range(attackData_SO.minDamage, attackData_SO.maxDamage);
        if(isCritical)
            coredamage *= attackData_SO.criticalMutiplier;
       // Debug.Log(coredamage);
            return coredamage;
    }
    #endregion

    #region Equip Weapon

    public void ChangeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquipWeapon(weapon);
    }
    public void EquipWeapon(ItemData_SO weapon)
    {
        if (weapon.weaponPrefab != null)
        {
            Instantiate(weapon.weaponPrefab, weaponSlot);
            //更新属性
            ////TODO:切换动画
            attackData_SO.ApplyWeaponData(weapon.weaponData);
            GetComponent<Animator>().runtimeAnimatorController = weapon.weaponAnimator;
            Debug.Log(GetComponent<Animator>().runtimeAnimatorController.name);
        }
        
    }

    public void UnEquipWeapon()
    {
        //Debug.Log(weaponSlot.transform.childCount);
        if (weaponSlot.transform.childCount != 0)
        {
            
            for (int i = 0; i < weaponSlot.transform.childCount; i++)
            {
                Destroy(weaponSlot.transform.GetChild(i).gameObject);
            }
        }
        attackData_SO.UnApplyWeaponData(baseattackData_SO);
        
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
        Debug.Log(GetComponent<Animator>().runtimeAnimatorController.name);
    }
    #endregion

    #region applyData Change

    public void ApplyHP(int hp)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + hp, MaxHealth);
    }

    #endregion
}
