using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Data",menuName = "Character Stats/Data")]
public class CharacterData_SO:ScriptableObject
{
    [Header("Stats info")]
    public float maxHP;
    public float currentHP;
    public float base_defence;
    public float current_defence;
    public float max_defence;
    [Header("Kill")]
    public int killPoint;
    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
    public float levelBuff;
    public float levelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }
    public void UpdateExp(int point )
    {
        currentExp += point;
        if (currentExp >= baseExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        //升级更新的方法
        currentLevel =Mathf.Clamp( currentLevel+ 1,0,maxLevel );
        baseExp +=(int)(baseExp * levelMultiplier);
        maxHP =(float)Math.Floor(maxHP * levelMultiplier);
        currentHP = maxHP;
        Debug.Log("Level up!:" + currentLevel + "  Maxhp:" + maxHP);
    }
}
