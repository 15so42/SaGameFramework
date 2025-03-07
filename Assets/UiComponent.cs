using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;


[System.Serializable]
public struct UiGroupConfig
{
    public string groupName;
    [Header("ui组的基础order")]
    public int baseOrder;

}

public class UiGroup
{
    
    public UiGroupConfig uiGroupConfig;
    public void Init(UiGroupConfig uiGroupConfig)
    {
        this.uiGroupConfig = uiGroupConfig;
    }

    public List<UiForm> uiForms = new List<UiForm>();

    public void AddUiForm(UiForm uiForm)
    {
        var canvas = uiForm.GetComponent<Canvas>();
        canvas.sortingOrder = uiGroupConfig.baseOrder + uiForms.Count;
        
        uiForms.Add(uiForm);

        uiForm.OnOpen();
    }

    public void OnUpdate()
    {
        foreach (var uiForm in uiForms)
        {
            uiForm.OnUpdate();
        }
    }

    public void RemoveUiForm(UiForm uiForm, bool resort = true)
    {
        
        uiForm.OnClose();
        
        GameEntry.GetGameComponent<UiComponent>().RecycleUiForm(uiForm);
        
        var removedIndex = uiForms.IndexOf(uiForm); 
        uiForms.RemoveAt(removedIndex);

        if (resort)
        {
            // 仅更新后续元素的层级
            for (int i = removedIndex; i < uiForms.Count; i++) {
                uiForms[i].GetComponent<Canvas>().sortingOrder = uiGroupConfig.baseOrder + i;
            }
        }
       
    }

    public bool HandleEscEvent()
    {
        for (int i = uiForms.Count - 1; i >= 0; i--)
        {
            if (uiForms[i].HandleEscEvent())
            {
                return true;
            }
        }

        return false;
    }

    public void RemoveAllUiForm()
    {
        for (int i = 0; i < uiForms.Count; i++)
        {
            RemoveUiForm(uiForms[i],false);
            i--;
        }
    }
}

public class UiComponent : IGameComponent
{

    public Transform root;//存放所有UiForm的Transform
    
    public List<UiGroupConfig> uiGroupConfigs = new List<UiGroupConfig>();

    private List<UiGroup> uiGroups = new List<UiGroup>();
    
    //存放每个uiGroup对应的Transform，添加到UIGroup中的ui会会放到对应的uiGroupRoot中
    private Dictionary<UiGroup, Transform> uiGroupRoots = new Dictionary<UiGroup, Transform>();

    //部分Ui可能会多次使用，所以放入对象池中，只有设置适用对象池的uiForm才会放入
    private Dictionary<int, ObjectPool<GameObject>> uiFormPools = new Dictionary<int, ObjectPool<GameObject>>();
    
    //Esc事件处理
    private PlayerInputActions playerInputActions;

    private void Start()
    {
        foreach (var uiGroupConfig in uiGroupConfigs)
        {
            var uiGroup = new UiGroup();

            uiGroup.Init(uiGroupConfig);
            uiGroups.Add(uiGroup);
            
            var go = new GameObject(uiGroupConfig.groupName);
            go.transform.SetParent(root);
            go.transform.localPosition=Vector3.zero;
            uiGroupRoots.Add(uiGroup,go.transform);

        }
    }

    protected override void Awake()
    {
        base.Awake();
        playerInputActions = new PlayerInputActions();
    }
    private void OnEnable()
    {
        playerInputActions.Enable();
        playerInputActions.Player.Esc.performed += HandleEscEvent;
    }

    private void Update()
    {
        foreach (var uiGroup in uiGroups)
        {
            uiGroup.OnUpdate();
        }
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
        playerInputActions.Player.Esc.performed += HandleEscEvent;
    }


    public UiForm ShowUiForm(int id, object userData)
    {
        var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<UiDataRow>(id);
        var uiGroup = uiGroups.Find(x => x.uiGroupConfig.groupName == dataRow.uiGroupName);
        if (uiGroup == null)
        {
            throw new KeyNotFoundException($"没有找到id为{id}的UI的目标UI组");
        }

        GameObject go = null;
        
        if (uiFormPools.ContainsKey(dataRow.id))
        {
            go = uiFormPools[dataRow.id].Get();
        }
        else
        {
            var pool = new ObjectPool<GameObject>(() =>
            {
                var tmpGo = GameObject.Instantiate(dataRow.uiPfb, uiGroupRoots[uiGroup]);
                tmpGo.SetActive(false);
                return tmpGo;
            },null,x => x.SetActive(false),null,true,3);
            uiFormPools.Add(dataRow.id,pool);
            
            go = pool.Get();
        }
        go.SetActive(true);
        
       
        go.transform.localPosition = Vector3.zero;

        var uiForm = go.GetComponent<UiForm>();
        uiForm.Init(dataRow);
        
        uiGroup.AddUiForm(uiForm);

       
        return uiForm;
    }

    public void CloseUiForm(UiForm uiForm)
    {
        var uiGroupName = uiForm.uiDataRow.uiGroupName;
        var uiGroup = uiGroups.Find(x => x.uiGroupConfig.groupName == uiGroupName);
        if (uiGroup!=null)
        {
            uiGroup.RemoveUiForm(uiForm);
            
        }
       
    }

    public void CloseAllUiForm()
    {
        foreach (var uiGroup in uiGroups)
        {
            uiGroup.RemoveAllUiForm();
        }
    }

    public void RecycleUiForm(UiForm uiForm)
    {
        //在生成uiForm的时候就已经生成了pool，所以不用再检查了
        if (uiFormPools.ContainsKey(uiForm.uiDataRow.id))
        {
            uiFormPools[uiForm.uiDataRow.id].Release(uiForm.gameObject);

        }
    }

    void HandleEscEvent(InputAction.CallbackContext context)
    {
        //处理Esc事件
        for (int i = uiGroups.Count - 1; i >= 0; i--)
        {
            if (uiGroups[i].HandleEscEvent())
            {
                break;
            }
        }
        
        TypeEventSystem.Send(new UiEscEvent());
    }
}
