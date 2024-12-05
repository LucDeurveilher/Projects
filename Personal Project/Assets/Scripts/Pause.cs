using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    Canvas canvas;
    GameManager.GameState lastGameState;
    Coroutine lastCoroutine;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    public void PauseGame()
    {
        lastGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameManager.GameState.Pause;

        canvas.enabled = true;

        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(Utility.FadeIn(canvas.GetComponent<CanvasGroup>(), 1, 0.15f,() => Time.timeScale = 0));
    }

    public void UnPauseGame()
    {
        GameManager.Instance.gameState = lastGameState;
        Time.timeScale = 1;

        if (lastCoroutine != null)
            StopCoroutine(lastCoroutine);
        lastCoroutine = StartCoroutine(Utility.FadeOut(canvas.GetComponent<CanvasGroup>(), 0, 0.15f,()=> canvas.enabled = false));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1 && GameManager.Instance.gameState == GameManager.GameState.Game)
            {
                PauseGame();
            }
            else if (GameManager.Instance.gameState == GameManager.GameState.Pause)
            {
                UnPauseGame();
            }
           
        }
    }
}
