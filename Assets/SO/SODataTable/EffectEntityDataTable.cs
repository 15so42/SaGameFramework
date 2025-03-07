using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectEntityDataRow:SoDataRow
{
    public int entityId;
    
    public float releaseTime=10f;
    
}

[CreateAssetMenu(menuName = "SoDataTable/粒子特效表")]
public class EffectEntityDataTable : SoDataTable<EffectEntityDataRow>
{
    
}
