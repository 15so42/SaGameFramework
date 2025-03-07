using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityProps 
{
    public Property<float> hp;
    public Property<float> mp;
    public Property<float> damageValue;//攻击力
    public Property<float> physicalDefense;
    public Property<float> magicPhysicalDefense;
    public Property<float> moveSpeed;
    public Property<float> actionSpeed;
    
}
