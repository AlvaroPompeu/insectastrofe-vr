using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonHorn : MonoBehaviour
{
    [SerializeField] AudioClip playerHitSFX;

    private AudioSource audioSource;
    private Collider hornCollider;

    private void Start()
    {
        audioSource = GetComponentInParent<AudioSource>();
        hornCollider = GetComponent<Collider>();
    }

    public void OpenCollider()
    {
        hornCollider.enabled = true;
        Invoke("CloseCollider", 0.5f);
    }

    private void CloseCollider()
    {
        hornCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.PlayOneShot(playerHitSFX);
            PlayerHelper player = other.gameObject.GetComponent<PlayerHelper>();
            player.TakeDamage();
        }
    }
}
