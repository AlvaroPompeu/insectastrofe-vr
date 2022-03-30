using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] Transform muzzleTransform;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] GameObject bulletDecalVFX;
    [SerializeField] GameObject objectHitVFX;

    private float range = 20f;
    private float bulletSpread = 0.05f;
    private int pelletCount = 6;
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

        // Send the raycasts
        for (int i = 0; i < pelletCount; i++)
        {
            if (Physics.Raycast(muzzleTransform.position, GenerateRaycastDirection(), out RaycastHit hitInfo, range))
            {
                // Instantiate and destroy (after a delay) the effects if a static object was hit
                if (hitInfo.transform.gameObject.isStatic)
                {
                    GameObject decal = Instantiate(bulletDecalVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    GameObject hit = Instantiate(objectHitVFX, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                    Destroy(decal, 10f);
                    Destroy(hit, 1f);
                }
            }
        }
    }

    private Vector3 GenerateRaycastDirection()
    {
        Vector3 startPoint = muzzleTransform.position;
        // Add a random spread at the foward direction
        Vector3 spreadVector = new Vector3(Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread), Random.Range(-bulletSpread, bulletSpread));
        Vector3 endPoint = startPoint + muzzleTransform.forward + spreadVector;

        return (endPoint - startPoint);
    }
}
