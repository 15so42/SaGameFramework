using System.Collections;
using System.Collections.Generic;
using System.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Pool;

public abstract class UiForm : MonoBehaviour
{
    [ReadOnly]
    public UiDataRow uiDataRow;
    public virtual void Init(UiDataRow uiDataRow)
    {
        this.uiDataRow = uiDataRow;
    }

    public abstract void OnOpen();


    public abstract void OnUpdate();


    public abstract void OnClose();


    public virtual void Close()
    {
        GameEntry.GetGameComponent<UiComponent>().CloseUiForm(this);
    }

    public abstract bool HandleEscEvent();
}
