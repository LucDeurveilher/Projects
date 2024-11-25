using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class UIUtility : MonoBehaviour
{
    [SerializeField] float fadeTime;
    [SerializeField] float transitionCameraTime;

    [SerializeField] GameObject actualCam = null;

    [SerializeField] GameObject nextCam = null;
    public void SwitchCamera()
    {
        if (actualCam != null && nextCam != null)
        {
            actualCam.SetActive(false);
            nextCam.SetActive(true);
        }
        else
        {
            Debug.LogWarning("One camemera not set");
        }

    }

    public void CanvasFadeOut(CanvasGroup canvasGroup)
    {
        StartCoroutine(Utility.FadeOut(canvasGroup, 0, fadeTime));
    }

    public void SwitchCamAfterFadeOut(CanvasGroup canvasGroup)
    {
        StartCoroutine(Utility.FadeOut(canvasGroup, 0, fadeTime, () => SwitchCamera()));
    }

    public void CanvasFadeIn(CanvasGroup canvasGroup)
    {
        Canvas canvas = canvasGroup.GetComponent<Canvas>();
        canvas.gameObject.SetActive(true);
        StartCoroutine(Utility.FadeIn(canvasGroup, 1, fadeTime)) ;
    }

    public void CanvasFadeInDelated(CanvasGroup canvasGroup)
    {
        if (transitionCameraTime != 0)
        {
            StartCoroutine(Utility.PlayFonctionAfterTimer(transitionCameraTime + fadeTime, () => CanvasFadeIn(canvasGroup)));
        }
        else
        {
            Debug.LogWarning("Transition 0 sec maybe a error");
        }
       
    }

    public void QuitGame()
    {
         Application.Quit();
    }

    public void SetGameStateGame()
    {
        GameManager.Instance.gameState = GameManager.GameState.Game;
    }

    public void SetGameStateMenu()
    {
        GameManager.Instance.gameState = GameManager.GameState.Menu;
    }

}
