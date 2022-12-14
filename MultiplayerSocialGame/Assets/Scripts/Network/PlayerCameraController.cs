using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerCameraController : NetworkBehaviour
{
    [Header("Camera")]
    [SerializeField] private Vector2 cameraVelocity = new Vector2(4f, 0.25f);
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private Transform playerCamera = null;
    [SerializeField] private float xClamp = 90f;

    private float xRotacao = 0f;
    private Camera cameraComponent;
    private AudioListener audioListener;

    public override void OnNetworkSpawn()
    {
        cameraComponent = playerCamera.GetComponent<Camera>();
        audioListener = playerCamera.GetComponent<AudioListener>();

        if (!IsOwner)
        {
            cameraComponent.enabled = false;
            audioListener.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;

            InputManager.Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
        }
    }

    /*public override void OnGainedOwnership()
    {
        cameraComponent = playerCamera.GetComponent<Camera>();
        audioListener = playerCamera.GetComponent<AudioListener>();
        cameraComponent.enabled = true;
        audioListener.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;

        InputManager.Controls.Player.Look.performed += ctx => Look(ctx.ReadValue<Vector2>());
    }*/

    /*public override void OnLostOwnership()
    {
        enabled = false;

        InputManager.Controls.Player.Look.performed -= ctx => Look(ctx.ReadValue<Vector2>());
    }*/

    public override void OnNetworkDespawn()
    {
        enabled = false;

        InputManager.Controls.Player.Look.performed -= ctx => Look(ctx.ReadValue<Vector2>());
    }

    private void Look(Vector2 lookAxis)
    {
        if (this == null)
        {
            return;
        }

        float deltaTime = Time.deltaTime;

        xRotacao -= lookAxis.y * cameraVelocity.y * deltaTime;
        xRotacao = Mathf.Clamp(xRotacao, -xClamp, xClamp);
        Vector2 rotacaoAlvo = transform.eulerAngles;
        rotacaoAlvo.x = xRotacao;

        playerCamera.eulerAngles = rotacaoAlvo;

        //visor.transform.position = transform.TransformPoint(playerCamera.transform.localPosition.x, playerCamera.transform.localPosition.y ,visorOffset.localPosition.z);
        //visor.transform.rotation = Quaternion.Euler(rotacaoAlvo.x,0,0);

        playerTransform.Rotate(0, lookAxis.x * cameraVelocity.x * deltaTime, 0f);
    }
}
