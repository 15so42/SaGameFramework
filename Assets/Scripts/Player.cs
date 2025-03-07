using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private BattleEntity entity;

    public float moveSpeed = 2;

    private Camera mainCam;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        
        mainCam=Camera.main;
        
    }

    private void OnEnable()
    {
        playerInputActions.Enable();
       
    }

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<BattleEntity>();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    private Vector2 cachedMoveInput;
    private Vector2 cachedRotateInput;

    private void Update()
    {
        cachedMoveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        cachedRotateInput = playerInputActions.Player.Rotate.ReadValue<Vector2>();

        
        if (!Application.isMobilePlatform && playerInputActions.Player.enabled)
        {
            Vector2 mousePos = playerInputActions.Player.MousePos.ReadValue<Vector2>();
            //使用射线决定方向
            var ray = mainCam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out var hit, 500,
                    LayerMask.GetMask("Ground")))
            {
                var dir = (hit.point - transform.position).normalized;
                cachedRotateInput = new Vector2(dir.x, dir.z);
            }
        }
    }

    private void FixedUpdate()
    {
        // 处理移动
        Vector3 moveDirection = new Vector3(cachedMoveInput.x, 0, cachedMoveInput.y);
        if (moveDirection.sqrMagnitude > 0.01f) {
            moveDirection = moveDirection.normalized * moveSpeed;
            entity.Move(moveDirection,Time.fixedDeltaTime);
        }

        // 处理旋转
        if (cachedRotateInput.sqrMagnitude > 0.01f) {
            Vector3 rotateDirection = new Vector3(cachedRotateInput.x, 0, cachedRotateInput.y);
            Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
            entity.Rotate(targetRotation, Time.fixedDeltaTime);
        }
    }
}
