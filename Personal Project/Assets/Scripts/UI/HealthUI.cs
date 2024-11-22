using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]Image imageHealth;
    [SerializeField]TextMeshProUGUI textHealth;

    public void UpdateUi(int value, int max)
    {
        if (value <= 0)
        {
            DestroyUI();
        }
        imageHealth.fillAmount = (float)value / (float)max;
        textHealth.text = $"{value}/{max}";
    }

    private void DestroyUI()
    {
        StartCoroutine(Utility.FadeOut(GetComponentInChildren<CanvasGroup>(),0f, 1f));
        Destroy(gameObject,1.5f);
    }
}
