using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShellGrabInteractable : XRGrabInteractable
{
    private void OnTriggerEnter(Collider other)
    {
        // Only load the shell if the player is holding it
        if (isSelected && other.gameObject.CompareTag("LoadingPort"))
        {
            Shotgun shotgunComponent = other.gameObject.GetComponentInParent<Shotgun>();
            
            // The method will return false if the magazine is full
            if (shotgunComponent.LoadBullet())
            {
                Destroy(gameObject);
            }
        }
    }
}