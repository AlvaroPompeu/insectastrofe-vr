using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollHelper : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] GameObject parent;
    [SerializeField] AudioClip bulletOnMetalSFX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayHardHitSFX()
    {
        audioSource.PlayOneShot(bulletOnMetalSFX);
    }

    public void PushBack(Rigidbody rigidBody, Vector3 direction)
    {
        rigidBody.AddForce(direction * 5, ForceMode.Impulse);
    }

    public void ToggleToRagdoll(Vector3 location, Quaternion rotation)
    {
        gameObject.SetActive(true);
        transform.position = location;
        transform.rotation = rotation;

        Destroy(parent, 20f);
    }
}
