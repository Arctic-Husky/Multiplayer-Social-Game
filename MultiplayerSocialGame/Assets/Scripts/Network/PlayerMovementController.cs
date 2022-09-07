using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovementController : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private CharacterController characController = null;

    private Vector2 inputAnterior;

    public override void OnNetworkSpawn()
    {
        enabled = true;

        InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Player.Move.canceled += ctx => ResetMovement();
    }

    /*public override void OnGainedOwnership() // mudar para OnGainedOwnership
    {
        if (!IsOwner) //
            Destroy(this);

        enabled = true;

        InputManager.Controls.Player.Move.performed += ctx => SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Player.Move.canceled += ctx => ResetMovement();
    }*/

    public override void OnNetworkDespawn()
    {
        enabled = false;

        InputManager.Controls.Player.Move.performed -= ctx => SetMovement(ctx.ReadValue<Vector2>());
        InputManager.Controls.Player.Move.canceled -= ctx => ResetMovement();
    }


    private void SetMovement(Vector2 movement)
    {
        if (!IsOwner)
            return;

        inputAnterior = movement;
    }

    
    private void ResetMovement()
    {
        if (!IsOwner)
            return;

        inputAnterior = Vector2.zero;
    }

    
    private void Update()
    {
        if (!IsOwner)
            return;

        Move();
    }

    
    private void Move()
    {
        Vector3 right = characController.transform.right;
        Vector3 forward = characController.transform.forward;

        right.y = 0f;
        forward.y = 0f;

        Vector3 movement = right.normalized * inputAnterior.x + forward.normalized * inputAnterior.y;

        characController.Move(movement * movementSpeed * Time.deltaTime);
    }
}
