using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveType
{
    Rigidbody,
    
}

public enum RotateType
{
    Normal,
}

[System.Serializable]
public class BattleEntityDataRow:SoDataRow
{
    public int entityId;

    public float hp=100;
    public float mp=100;
    public float baseDamage=5;
    public float pDefense=0;
    public float mDefense=0;
    public float moveSpeed=1;
    public float actionSpeed=1;

    public MoveType moveType;
    public RotateType rotateType;

    public int weaponId;
   
    [Header("自带Buff")]
    public string buffs;
}

[CreateAssetMenu(menuName = "SoDataTable/战斗单位表")]
public class BattleEntityDataTable : SoDataTable<BattleEntityDataRow>
{
    
}
