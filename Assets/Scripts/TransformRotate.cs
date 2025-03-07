using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformRotate : UnitRotate
{

    public float lerpTime=5;
    protected override void RotateLogic(Quaternion targetRotation,float deltaTime)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,lerpTime*deltaTime);
    }
}
