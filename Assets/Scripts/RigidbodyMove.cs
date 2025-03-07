using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMove : UnitMove
{

  
    private Rigidbody rb;
    [SerializeField] private float acceleration = 10f; // 加速度(m/s²)

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


  
    protected override void MoveLogic(Vector3 targetVelocity, float deltaTime) {
        // 根据加速度逐步逼近目标速度
        rb.velocity = Vector3.MoveTowards(
            rb.velocity, 
            targetVelocity, 
            acceleration * deltaTime // 每帧允许的速度变化量
        );
    }
}
