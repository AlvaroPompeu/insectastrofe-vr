using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAction : MonoBehaviour
{
    private float zBoundary = 2.2f;

    private Shotgun shotgun;
    private AudioSource audioSource;
    [SerializeField] AudioClip pumpActionStartSFX;
    [SerializeField] AudioClip pumpActionEndSFX;
    [SerializeField] GameObject emptyShellPrefab;
    [SerializeField] Transform bulletEjector;

    void Start()
    {
        shotgun = GetComponentInParent<Shotgun>();
        audioSource = GetComponent<AudioSource>();
    }

    void LateUpdate()
    {
        ApplyTransformBoundaries();
    }

    private void ApplyTransformBoundaries()
    {
        if (transform.localPosition.z < 0)
        {
            transform.localPosition = Vector3.zero;
        }
        else if (transform.localPosition.z > zBoundary)
        {
            transform.localPosition = new Vector3(0, 0, zBoundary);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PumpAction"))
        {
            audioSource.PlayOneShot(pumpActionStartSFX);
            if (shotgun.hasEmptyShell)
            {
                EjectEmptyShell();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PumpAction"))
        {
            audioSource.PlayOneShot(pumpActionEndSFX);
            if (shotgun.magCount > 0)
            {
                shotgun.shotReady = true;
            }
        }
    }

    private void EjectEmptyShell()
    {
        GameObject shell = Instantiate(emptyShellPrefab, bulletEjector.position, bulletEjector.rotation);
        Rigidbody shellRB = shell.GetComponent<Rigidbody>();
        shellRB.AddForce(bulletEjector.right * 3, ForceMode.Impulse);
        Destroy(shell, 15);
        shotgun.hasEmptyShell = false;
    }
}
