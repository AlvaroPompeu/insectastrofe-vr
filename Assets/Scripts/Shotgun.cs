using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] Transform muzzleTransform;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip shootSFX;

    private float range = 30f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Shoot()
    {
        // Play the muzzle flash and the SFX
        muzzleFlash.Play();
        audioSource.PlayOneShot(shootSFX);

        // Send the raycast
        RaycastHit hitInfo;
        if (Physics.Raycast(muzzleTransform.position, muzzleTransform.forward, out hitInfo, range))
        {
            Debug.Log(hitInfo.transform.name);
        }
    }
}
