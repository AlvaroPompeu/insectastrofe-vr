using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellBox : MonoBehaviour
{
    private BulletHolster bulletHolster;

    private void Start()
    {
        bulletHolster = GameObject.Find("XR").GetComponentInChildren<BulletHolster>();
    }

    public void PickShellBox()
    {
        bulletHolster.RefillBelt();
        Destroy(gameObject);
    }
}
