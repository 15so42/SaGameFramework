using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class GameEntry : MonoBehaviour
{
    private static DataComponent dataComponent;
    private static UiComponent uiComponent;
    private static ProcedureComponent procedureComponent;
    private static EntityComponent entityComponent;
    private static SoDataTableComponent soDataTableComponent;
    private static CameraShakeComponent cameraShakeComponent;
    private static SoundComponent soundComponent;

    public int mobileTargetFrame=60;
    public int pcTargetFrame=120;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = Application.isMobilePlatform ? mobileTargetFrame : pcTargetFrame;
    }

    // 使用接口类型存储，但通过泛型精确检索
    private static readonly Dictionary<Type, IGameComponent> gameComponents = new();

    public static void RegisterComponent<T>(T component) where T : IGameComponent
    {
        // 关键修改：通过运行时实例获取具体类型
        Type concreteType = component.GetType();
        
        if (!gameComponents.ContainsKey(concreteType))
        {
            gameComponents.Add(concreteType, component);
            //Debug.Log($"成功注册 {concreteType.Name}");
        }
        else
        {
            Debug.LogWarning($"组件 {concreteType.Name} 已存在，跳过重复注册");
        }
    }

    // 获取时直接返回具体类型
    public static T GetGameComponent<T>() where T : IGameComponent
    {
        if (gameComponents.TryGetValue(typeof(T), out IGameComponent component))
        {
            return (T)component; // 这里强制转换是安全的
        }
        throw new KeyNotFoundException($"{typeof(T).Name} 没有注册");
    }


    public static DataComponent Data {
        get {
            if (dataComponent == null)
            {
                dataComponent = GetGameComponent<DataComponent>();
            }

            return dataComponent;
        }
    }
    
    public static UiComponent Ui {
        get {
            if (uiComponent == null)
            {
                uiComponent = GetGameComponent<UiComponent>();
            }

            return uiComponent;
        }
    }
    
    public static ProcedureComponent Procedure {
        get {
            if (procedureComponent == null)
            {
                procedureComponent = GetGameComponent<ProcedureComponent>();
            }

            return procedureComponent;
        }
    }
    
    public static EntityComponent Entity {
        get {
            if (entityComponent == null)
            {
                entityComponent = GetGameComponent<EntityComponent>();
            }

            return entityComponent;
        }
    }
    
    public static SoDataTableComponent SoDataTable {
        get {
            if (soDataTableComponent == null)
            {
                soDataTableComponent = GetGameComponent<SoDataTableComponent>();
            }

            return soDataTableComponent;
        }
    }
    
    public static CameraShakeComponent CameraShake {
        get {
            if (cameraShakeComponent == null)
            {
                cameraShakeComponent = GetGameComponent<CameraShakeComponent>();
            }

            return cameraShakeComponent;
        }
    }
    
    public static SoundComponent Sound {
        get {
            if (soundComponent == null)
            {
                soundComponent = GetGameComponent<SoundComponent>();
            }

            return soundComponent;
        }
    }
    
    

}
