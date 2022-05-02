using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BulletHolster : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] GameObject[] sockets;
    [SerializeField] AudioClip ammoPickupSFX;

    private AudioSource audioSource;
    private float baseRotateSpeed = 50f;
    private float yOffset = 0.55f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - yOffset, mainCamera.transform.position.z);

        // The holster rotate speed must adapt as the head rotation is closer or further
        float finalRotateSpeed = CalculateFinalRotateSpeed();
        float step = finalRotateSpeed * Time.deltaTime;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), step);
    }

    private float CalculateFinalRotateSpeed()
    {
        float rotationDifference = Mathf.Abs(mainCamera.transform.eulerAngles.y - transform.eulerAngles.y);

        if (rotationDifference <= 20)
        {
            return (baseRotateSpeed / 4);
        }
        else if (rotationDifference <= 40)
        {
            return (baseRotateSpeed / 2);
        }
        else if (rotationDifference <= 60)
        {
            return baseRotateSpeed;
        }
        else
        {
            return (baseRotateSpeed * 2);
        }
    }

    public void RefillBelt()
    {
        audioSource.PlayOneShot(ammoPickupSFX);

        for (int i = 0; i < sockets.Length; i++)
        {
            // Check if the socket is empty
            IXRSelectInteractable obj = sockets[i].GetComponent<XRSocketInteractor>().GetOldestInteractableSelected();
            if (obj == null)
            {
                // Instantiate the shell in the socket position
                Instantiate(shellPrefab, sockets[i].transform.position, sockets[i].transform.rotation);
            }
        }
    }
}