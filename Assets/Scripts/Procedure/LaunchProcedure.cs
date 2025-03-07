using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Timer = UnityTimer.Timer;


public class LaunchProcedure : Procedure
{
    
    public override void OnEnter()
    {
        Timer.Register(0.5f, () =>
        {
            GameEntry.GetGameComponent<ProcedureComponent>().ChangeScene("MainMenu", new MainMenuProcedure());
        });

    }

    public override void OnUpdate()
    {
      
    }

    public override void OnExit()
    {
        
    }
}
