using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUiForm : UiForm
{
    public Button pvpButton;
    public override void Init(UiDataRow uiDataRow)
    {
        base.Init(uiDataRow);
    }

    public override void OnOpen()
    {
       pvpButton.onClick.AddListener(() =>
       {
           GameEntry.GetGameComponent<DataComponent>().SetData("TargetGameMode", new PVPGameMode());
           GameEntry.GetGameComponent<ProcedureComponent>().ChangeScene("Battle",new BattleProcedure());
       });
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnClose()
    {
       
    }

    public override bool HandleEscEvent()
    {
        return false;
    }
}
