using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;


public class EntityComponent : IGameComponent
{

   public Transform root;//生成的所有Entity放在root下面
   public List<Entity> entities = new List<Entity>();
   
   private Dictionary<int, ObjectPool<GameObject>> entityPools = new Dictionary<int, ObjectPool<GameObject>>();

   public Entity ShowEntity<T>(int id, Vector3 pos, Quaternion rotation, object userData) where T : Entity
   {
      var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<EntityDataRow>(id);
    
      GameObject targetGo = null;

      if (dataRow.usePool)
      {
         // 使用数据表ID作为对象池键
         if (entityPools.TryGetValue(dataRow.id, out var targetPool))
         {
            targetGo = targetPool.Get();
         }
         else
         {
            // 创建新的对象池并缓存
            targetPool = new ObjectPool<GameObject>(
               createFunc: () => 
               {
                  var go = GameObject.Instantiate(dataRow.pfb);
                  go.SetActive(false); // 初始状态设为非激活
                  return go;
               },
               actionOnGet: go => 
               {
                  go.transform.position = pos;
                  go.transform.rotation = rotation;
                  go.SetActive(true);
               }
            );

            entityPools.Add(dataRow.id, targetPool);
            targetGo = targetPool.Get();
         }
      }
      else
      {
         // 非池化直接实例化
         targetGo = GameObject.Instantiate(dataRow.pfb, pos, rotation);
      }

      targetGo.transform.SetParent(root);
      // 获取或添加Entity组件
      var entityComp = targetGo.GetComponent<T>() ?? targetGo.AddComponent<T>();
    
      // 使用原型模式避免数据引用问题
      var clonedData = dataRow.CloneViaSerialization();
      entityComp.Init(clonedData, userData);
      entityComp.OnShow();
      
      return entityComp;
   }

   private void Update()
   {
      foreach (var entity in entities)//不会存在已经hide的Entity
      {
         entity.OnUpdate();
      }
   }

   public Entity ShowBattleEntity(int battleEntityId, Vector3 pos,Quaternion rotation,object userData)
   {
      var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<BattleEntityDataRow>(battleEntityId);
      return ShowEntity<BattleEntity>(dataRow.entityId, pos, rotation, new Tuple<BattleEntityDataRow,object>(dataRow,userData));
   }
   
   public Entity ShowEffectEntity(int effectEntityId, Vector3 pos,Quaternion rotation,object userData)
   {
      var dataRow = GameEntry.GetGameComponent<SoDataTableComponent>().GetSoDataRow<EffectEntityDataRow>(effectEntityId);
      return ShowEntity<EffectEntity>(dataRow.entityId, pos, rotation, new Tuple<EffectEntityDataRow,object>(dataRow,userData));
   }

   public Entity ShowCameraEntity(int cameraEntityId, Vector3 pos, Quaternion rotation, object userData)
   {
      return ShowEntity<CameraEntity>(cameraEntityId, pos, rotation, userData);
   }


   public void HideEntity(Entity entity)
   {
      
      entity.OnHide();
      if (entityPools.ContainsKey(entity.dataRow.id))
      {
         entityPools[entity.dataRow.id].Release(entity.gameObject);
      }
      
      entity.gameObject.SetActive(false);
      entities.Remove(entity);
   }

   public void HideAllEntity()
   {
      for (int i = 0; i < entities.Count; i++)
      {
         HideEntity(entities[i]);
         i--;
      }
   }
  
}
