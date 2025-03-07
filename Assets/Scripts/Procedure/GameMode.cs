using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode
{
   public abstract bool IsGameOver();//包含所有情形，输赢平。

   public abstract void OnEnter();
   public abstract void OnUpdate();
   public abstract void OnExit();

}
