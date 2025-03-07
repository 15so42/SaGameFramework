using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class UnitMove : MonoBehaviour
{

    public bool moveAble = true;

    public void Move(Vector3 velocity,float deltaTime)
    {
        if (moveAble)
        {
            MoveLogic(velocity,deltaTime);
        }
    }

    protected abstract void MoveLogic(Vector3 velocity,float deltaTime);


}
