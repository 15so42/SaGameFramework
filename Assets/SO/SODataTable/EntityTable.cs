using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityDataRow:SoDataRow
{
    public GameObject pfb;
    public bool usePool;
    
    [Header("相机跟随绑点设置")]
    public string lookAtTransPointKey="LookAtTransPoint";
    public string followTransPointKey="FollowTransPoint";

}
[CreateAssetMenu(menuName = "SoDataTable/Entity表")]
public class EntityTable : SoDataTable<EntityDataRow>
{
}
