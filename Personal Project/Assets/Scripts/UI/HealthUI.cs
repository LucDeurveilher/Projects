using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField]Image imageHealth;
    [SerializeField] Image effectImageHealth;
    [SerializeField]TextMeshProUGUI textHealth;

    public void UpdateUi(int value, int max)
    {
        if (value <= 0)
        {
            DestroyUI();
        }
        imageHealth.fillAmount = (float)value / (float)max;
        textHealth.text = $"{value}/{max}";

        StartCoroutine(HitEffectHealthBar());
    }

    private void DestroyUI()
    {
        StartCoroutine(Utility.FadeOut(GetComponentInChildren<CanvasGroup>(),0f, 1f));
        Destroy(gameObject,1.5f);
    }

    IEnumerator HitEffectHealthBar()
    {
        while (effectImageHealth.fillAmount > imageHealth.fillAmount)
        {
            effectImageHealth.fillAmount-=Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
