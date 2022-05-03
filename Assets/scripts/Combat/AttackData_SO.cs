using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Data", menuName = "Attack/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Stats info")]
    public float maxDamage;
    public float minDamage;
    public float attackRange;
    public float skillRange;
    public float attackCD;
    public float skillCD;
    public float criticalMutiplier;
    public float criticalChance;

    public void ApplyWeaponData(AttackData_SO weapon)
    {
        maxDamage+= weapon.maxDamage;
        minDamage += weapon.minDamage;
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        attackCD = weapon.attackCD;
        skillCD = weapon.skillCD;
        criticalMutiplier = weapon.criticalMutiplier;
        criticalChance = weapon.criticalChance;

    }
    public void UnApplyWeaponData(AttackData_SO weapon)
    {
        maxDamage= weapon.maxDamage;
        minDamage = weapon.minDamage;
        attackRange = weapon.attackRange;
        skillRange = weapon.skillRange;
        attackCD = weapon.attackCD;
        skillCD = weapon.skillCD;
        criticalMutiplier = weapon.criticalMutiplier;
        criticalChance = weapon.criticalChance;

    }
}
