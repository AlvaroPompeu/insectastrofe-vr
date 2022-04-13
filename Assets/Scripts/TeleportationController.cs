using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class TeleportationController : MonoBehaviour
{
    [SerializeField] InputActionReference teleportActivationReference;
    [Space]
    [SerializeField] UnityEvent onTeleportActivate;
    [SerializeField] UnityEvent onTeleportCancel;

    private void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", 0.1f);
    }

    private void DeactivateTeleporter()
    {
        onTeleportCancel.Invoke();
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        onTeleportActivate.Invoke();
    }
}
