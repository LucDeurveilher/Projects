using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterToolTip : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public CharacterStatsToolTipDisplay toolTip;
    public float fadeTime = 0.1f;

    private void Awake()
    {
        toolTip = FindObjectOfType<CharacterStatsToolTipDisplay>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (toolTip != null)
        {
            toolTip.SetStatsText(GetComponent<CharacterStats>());
            StartCoroutine(Utility.FadeIn(toolTip.canvasGroup, 1.0f, fadeTime));
        }
        else
        {
            toolTip = FindObjectOfType<CharacterStatsToolTipDisplay>();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Utility.FadeOut(toolTip.canvasGroup, 0.0f, fadeTime));
    }
}
