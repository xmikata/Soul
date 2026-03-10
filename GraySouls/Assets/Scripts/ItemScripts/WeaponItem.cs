using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("伤害")]
    public int baseDamage;
    public int criticalDamageMuiltiplier=2;

    [Header("防御强度")]
    public int physicalDamageAbsorption;

    [Header("削韧")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("闲置动画")]
    public string right_hand_idle;
    public string left_hand_idle;
    public string th_idle;

    [Header("攻击动画")]//动画名
    public string One_Light_Attack_1;
    public string One_Light_Attack_2;
    public string One_Heavy_Attack_1;
    public string One_Heavy_Attack_2;
    public string Two_Light_Attack_1;
    public string Two_Light_Attack_2;
    public string Two_Light_Attack_3;
    public string Two_Heavy_Attack_1;

    [Header("武器战技")]
    public string weapon_art;

    [Header("耐力消耗")]
    public int baseStamina;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;

    [Header("武器类型")]
    public bool isMeleeWeapon;
    public bool isShieldWeapon;
}
