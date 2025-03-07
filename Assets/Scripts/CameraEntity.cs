using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraEntity : Entity
{
   private Entity targetEntity;

   private CinemachineVirtualCamera cinemachineVirtualCamera;

   public override void Init(EntityDataRow entityDataRow, object userData)
   {
      base.Init(entityDataRow, userData);
      cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
   }

   public void SetTarget(Entity entity)
   {
      var entityData = entity.dataRow as EntityDataRow;

      var lookAtTransBp = entity.bindPointManager.GetBindPoint(entityData.lookAtTransPointKey);
      var followTransBp = entity.bindPointManager.GetBindPoint(entityData.followTransPointKey);
      
      
      if (lookAtTransBp != null)
      {
         cinemachineVirtualCamera.LookAt = lookAtTransBp.transform;
      }

      if (followTransBp != null)
      {
         cinemachineVirtualCamera.Follow = followTransBp.transform;
      }
      
   }
}
