using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleProcedure : Procedure
{
    public GameMode gameMode;
    
    public override void OnEnter()
    {

        var targetGameMode = GameEntry.GetGameComponent<DataComponent>().GetData<GameMode>("TargetGameMode", new PVPGameMode());

        gameMode = targetGameMode;
        gameMode.OnEnter();
    }

    public override void OnUpdate()
    {
        gameMode.OnUpdate();
    }

    public override void OnExit()
    {
       gameMode.OnExit();
    }
}
