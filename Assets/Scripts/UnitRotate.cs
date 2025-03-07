using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UnitRotate : MonoBehaviour
{

    public bool rotateAble=true;

    public void Rotate(Quaternion targetRotation,float deltaTime)
    {
        if (rotateAble)
        {
            RotateLogic(targetRotation,deltaTime);
        }
    }
    protected abstract void RotateLogic(Quaternion targetRotation,float deltaTime);

}
