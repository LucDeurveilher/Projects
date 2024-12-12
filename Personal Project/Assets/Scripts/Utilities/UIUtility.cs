using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class UIUtility : MonoBehaviour
{
    [SerializeField] public float fadeTime;
    [SerializeField] float transitionCameraTime;

    [SerializeField] GameObject nextCam = null;
    public void SwitchCamera()
    {
        CinemachineCore.Instance.GetActiveBrain(0).m_DefaultBlend.m_Time = 2f;

        GameObject actualCam = CinemachineCore.Instance.GetActiveBrain(0).ActiveVirtualCamera.VirtualCameraGameObject;

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

    public void SwitchCamAndDisableCanva(CanvasGroup canvasGroup)
    {
        StartCoroutine(Utility.PlayFonctionAfterTimer(transitionCameraTime + fadeTime + 0.5f, () =>canvasGroup.gameObject.SetActive(false)));
        SwitchCamera();
    }

    public void SwitchCamAfterFadeOut(CanvasGroup canvasGroup)
    {
        StartCoroutine(Utility.FadeOut(canvasGroup, 0, fadeTime, () => SwitchCamAndDisableCanva(canvasGroup)));
    }

    public void CanvasFadeIn(CanvasGroup canvasGroup)
    {
        StartCoroutine(Utility.FadeIn(canvasGroup, 1, fadeTime)) ;
    }

    public void CanvasFadeInDelated(CanvasGroup canvasGroup)
    {
        canvasGroup.GetComponent<Canvas>().gameObject.SetActive(true);
       
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
