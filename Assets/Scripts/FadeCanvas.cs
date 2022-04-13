using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    private float fadeDuration = 0.5f;
    private float step;
    private CanvasGroup canvasGroup;

    private bool fadingIn = false;
    private bool fadingOut = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        step = Time.deltaTime / fadeDuration;
    }

    void Update()
    {
        if (fadingIn)
        {
            canvasGroup.alpha += step;
            if (canvasGroup.alpha >= 1)
            {
                fadingIn = false;
                fadingOut = true;
            }
        }
        else if (fadingOut)
        {
            canvasGroup.alpha -= step;
            if (canvasGroup.alpha <= 0)
            {
                fadingOut = false;
            }
        }
    }

    public void Fade()
    {
        // Prevent setting to true in the middle of any fade
        if (!fadingIn && !fadingOut)
        {
            fadingIn = true;
        }
    }
}
