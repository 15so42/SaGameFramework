using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPGameMode : GameMode
{
    
    public override bool IsGameOver()
    {
        return false;
    }

    public override void OnEnter()
    {
        var playerEntity=GameEntry.Entity.ShowBattleEntity(0, Vector3.zero, Quaternion.identity, null) as BattleEntity;
        GameEntry.Data.SetData("PlayerEntity",playerEntity);

        var cameraEntity = GameEntry.Entity.ShowCameraEntity(1, new Vector3(0, 10, -10), Quaternion.identity, null) as CameraEntity;
        GameEntry.Data.SetData("CameraEntity",cameraEntity);
        cameraEntity.SetTarget(playerEntity);
    }

    public override void OnUpdate()
    {
      
    }

    public override void OnExit()
    {
        
    }
}
