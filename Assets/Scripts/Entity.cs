using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour//Entity和UiForm不同，每个UIForm使用单独的UiForm子类，Entity则不同，Entity是一类Entity使用一种Entity子类
{
    public EntityDataRow dataRow;//entityDataRow是class类型，不是so

    public BindpointManager bindPointManager;
    private object userData;
    public virtual void Init(EntityDataRow entityDataRow,object userData)//传入的是克隆后的，不用担心引用问题
    {
        this.dataRow = entityDataRow;
        this.userData = userData;

        bindPointManager= GetComponent<BindpointManager>();
        if (bindPointManager==null)
        {
            bindPointManager = gameObject.AddComponent<BindpointManager>();
        }
        bindPointManager.Init();
    }

    public virtual void OnShow()
    {
        
    }

    public virtual void OnUpdate()
    {
        
    }
    
    public virtual void OnHide()
    {
        
    }

    public virtual void Hide()
    {
        GameEntry.GetGameComponent<EntityComponent>().HideEntity(this);
    }

    
}
