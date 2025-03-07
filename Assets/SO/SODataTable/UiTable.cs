using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UiDataRow:SoDataRow
{
    public GameObject uiPfb;
    public string uiGroupName="Default";
    public bool pauseCoveredUiForm;
    
}
[CreateAssetMenu(menuName = "SoDataTable/Ui表")]
public class UiTable : SoDataTable<UiDataRow>
{
   
}
