using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    Canvas canvas;
    GameManager.GameState lastGameState;
    private void Start()
    {
        canvas = GetComponent<Canvas>();
    }
    public void PauseGame()
    {
        lastGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameManager.GameState.Pause;
        canvas.enabled = true;
        Time.timeScale = 0;
    }

    public void UnPauseGame()
    {
        GameManager.Instance.gameState = lastGameState;
        canvas.enabled = false;
        Time.timeScale = 1;
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
