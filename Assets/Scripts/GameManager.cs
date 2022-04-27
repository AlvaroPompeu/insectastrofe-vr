using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int metalonKillCount = 0;
    public bool IsGameActive { get; private set; }

    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] XRDirectInteractor[] directInteractors;

    private void Awake()
    {
        Instance = this;
        IsGameActive = true;
    }

    public void EnableGameOverScreen()
    {
        gameOverCanvas.SetActive(true);
        gameOverText.text = metalonKillCount.ToString() +" Metalons were killed.\nBetter luck next time.";
        IsGameActive = false;
        DisableDirectInteractors();
    }

    private void DisableDirectInteractors()
    {
        for (int i = 0; i < directInteractors.Length; i++)
        {
            directInteractors[i].enabled = false;
        }
    }
}
