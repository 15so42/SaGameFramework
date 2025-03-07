using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEntity : Entity
{
    private BattleEntityDataRow battleEntityDataRow;

    private UnitMove unitMove;
    private UnitRotate unitRotate;
    
    public override void Init(EntityDataRow entityDataRow, object userData)
    {
        base.Init(entityDataRow, userData);
        
        battleEntityDataRow=(userData as Tuple<BattleEntityDataRow,object>)?.Item1;
        
        if (battleEntityDataRow.moveType == MoveType.Rigidbody)
        {
            unitMove=gameObject.AddComponent<RigidbodyMove>();
        }

        unitRotate=gameObject.AddComponent<TransformRotate>();

    }

    public void Move(Vector3 velocity,float deltaTime)
    {
        unitMove.Move(velocity,deltaTime);
    }
    
    public void Rotate(Quaternion targetRotation,float deltaTime)
    {
        unitRotate.Rotate(targetRotation,deltaTime);
    }
}
