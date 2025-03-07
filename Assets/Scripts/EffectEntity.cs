using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTimer;

public class EffectEntity : Entity
{
    private EffectEntityDataRow effectEntityDataRow;
    public override void Init(EntityDataRow entityDataRow, object userData)
    {
        base.Init(entityDataRow, userData);
        effectEntityDataRow=(userData as Tuple<EffectEntityDataRow,object>)?.Item1;

        Timer.Register(effectEntityDataRow.releaseTime, () =>
        {
            Hide();
        });
    }
}
