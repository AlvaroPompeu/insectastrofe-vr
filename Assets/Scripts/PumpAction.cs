using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpAction : MonoBehaviour
{
    private float zBoundary = 2.2f;

    private AudioSource audioSource;
    [SerializeField] AudioClip pumpActionStartSFX;
    [SerializeField] AudioClip pumpActionEndSFX;

    void Start()
    {
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
        audioSource.PlayOneShot(pumpActionStartSFX);
    }

    private void OnTriggerExit(Collider other)
    {
        audioSource.PlayOneShot(pumpActionEndSFX);
    }
}
