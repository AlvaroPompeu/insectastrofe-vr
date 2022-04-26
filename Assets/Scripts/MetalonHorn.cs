using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalonHorn : MonoBehaviour
{
    private Collider hornCollider;

    private void Start()
    {
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
            PlayerHelper player = other.gameObject.GetComponent<PlayerHelper>();
            player.TakeDamage();
        }
    }
}
