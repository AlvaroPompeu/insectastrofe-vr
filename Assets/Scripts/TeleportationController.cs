using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportationController : MonoBehaviour
{
    [SerializeField] Transform XROrigin;
    [SerializeField] FadeCanvas fadeCanvas;
    [SerializeField] InputActionReference teleportActivationReference;
    [Space]
    [SerializeField] UnityEvent onTeleportActivate;
    [SerializeField] UnityEvent onTeleportCancel;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool teleportActivated = false;
    private bool teleportReady = true;
    private float teleportCooldown = 0.5f;

    private void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", 0.03f);
    }

    private void DeactivateTeleporter()
    {
        onTeleportCancel.Invoke();
        targetPosition = XROrigin.position;
        TeleportHandler();
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        if (teleportReady && GameManager.Instance.IsGameActive)
        {
            onTeleportActivate.Invoke();
            originalPosition = XROrigin.position;
            teleportActivated = true;
        }
    }

    // Check if the player position has changed and fade if so, also manage the teleport cooldown
    private void TeleportHandler()
    {
        if (originalPosition != targetPosition && teleportActivated)
        {
            fadeCanvas.Fade();
            teleportActivated = false;
            teleportReady = false;
            StartCoroutine(TeleportCooldown());
        }
    }

    IEnumerator TeleportCooldown()
    {
        yield return new WaitForSeconds(teleportCooldown);
        teleportReady = true;
    }
}
