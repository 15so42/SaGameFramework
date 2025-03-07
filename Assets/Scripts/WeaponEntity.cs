using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponDataRow
{
    public string weaponName;
   
    [Header("属性变化")]
    public float hpAdd=0;
    public float mpAdd=0;
    public float baseDamageAdd=0;
    public float pDefenseAdd=0;
    public float mDefenseAdd=0;
    public float moveSpeedAdd=0;
    public float actionSpeedAdd=0;
    
    public float hpTimes=0;
    public float mpTimes=0;
    public float baseDamageTimes=0;
    public float pDefenseTimes=0;
    public float mDefenseTimes=0;
    public float moveSpeedTimes=0;
    public float actionSpeedTimes=0;
    
    [Header("可以有多个，本项目只使用第一个")]
    public string[] skills;
    
    
}

public class WeaponEntity : MonoBehaviour
{
  
}
