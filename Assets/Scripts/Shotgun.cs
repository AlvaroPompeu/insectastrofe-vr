using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] Transform muzzleTransform;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] AudioClip shootSFX;
    [SerializeField] AudioClip emptyShootSFX;
    [SerializeField] AudioClip reloadSFX;
    [SerializeField] GameObject bulletDecalVFX;
    [SerializeField] GameObject objectHitVFX;

    private float range = 20f;
    private float bulletSpread = 0.05f;
    private int minPelletCount = 8;
    private int maxPelletCount = 12;
    private int magSize = 6;
    public int magCount { get; private set; }
    private AudioSource audioSource;
    public bool shotReady = true;
    public bool hasEmptyShell = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        magCount = magSize;
    }

    public void Shoot()
    {
        if (magCount > 0 && shotReady)
        {
            // Play the muzzle flash and the SFX
            muzzleFlash.Play();
            audioSource.PlayOneShot(shootSFX);

            // Send the raycasts
            int pelletCount = Random.Range(minPelletCount, maxPelletCount);
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

            // Spend ammo and disable the gun
            magCount--;
            shotReady = false;
            hasEmptyShell = true;
        }
        else
        {
            audioSource.PlayOneShot(emptyShootSFX);
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

    public bool LoadBullet()
    {
        // Let the player reload
        if (magCount < magSize)
        {
            audioSource.PlayOneShot(reloadSFX);
            magCount++;
            return true;
        }
        else
        {
            return false;
        }
        
    }
}
