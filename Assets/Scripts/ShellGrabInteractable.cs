using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShellGrabInteractable : XRGrabInteractable
{
    private void OnTriggerEnter(Collider other)
    {
        // Only load the shell if the player is holding it
        if (IsInHand() && other.gameObject.CompareTag("LoadingPort"))
        {
            Shotgun shotgunComponent = other.gameObject.GetComponentInParent<Shotgun>();
            
            // The method will return false if the magazine is full
            if (shotgunComponent.LoadBullet())
            {
                Destroy(gameObject);
            }
        }
    }

    private bool IsInHand()
    {
        if (isSelected)
        {
            // Check if the shell is in the socket (belt holster)
            if (interactorsSelecting[0].GetType().ToString() != "UnityEngine.XR.Interaction.Toolkit.XRSocketInteractor")
            {
                // The shell is in the player's hand
                return true;
            }
        }

        return false;
    }
}