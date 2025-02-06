using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private int turn = 0;
    [SerializeField] private bool playerTurn = true;
    [SerializeField] AttackManager attackManager;
    [SerializeField] Button buttonTurn;
    [SerializeField] Button buttonDraw;
    [SerializeField] TextMeshProUGUI textTurn;//ajouter

    [SerializeField] GameObject gameCamera;
    [SerializeField] GameObject combatCamera;

    [SerializeField] Wheel wheel;

    public AIBase AI;

    static public Action EffectTurn;

    public int starter = 0;

    UIUtility uIUtility;
    [SerializeField] CanvasGroup gameCanvaGroup;

    private void Start()
    {
        GameManager.Instance.PlayerTurn = false;
       
        uIUtility = gameObject.AddComponent<UIUtility>();
        uIUtility.fadeTime = 0.25f;
    }

    public void OpenTheWheel()
    {
        StartCoroutine(Utility.PlayFonctionAfterTimer(3,() =>wheel.OpenWheel()));
    }

    private void Update()
    {
        if (buttonDraw.interactable != !GameManager.Instance.CardPlayed)
            buttonDraw.interactable = !GameManager.Instance.CardPlayed;
    }

    public void RandomStarter()
    {

        if (starter == 0)
        {
            playerTurn = true;
        }
        else
        {
            playerTurn = false;
            GameManager.Instance.CardPlayed = true;
            StartCoroutine(Utility.PlayFonctionAfterTimer(1, () => AI.PlayAI()));//mettre le timer dans l'AI
        }

        GameManager.Instance.PlayerTurn = playerTurn;
        SetText();
    }

    public void EndTurn()
    {
        if (playerTurn)
        {
            GameManager.Instance.CardPlayed = true;
        }
        else
        {
            GameManager.Instance.CardPlayed = false;
        }


        playerTurn = playerTurn ? false : true;
        GameManager.Instance.PlayerTurn = playerTurn;


        turn++;
        SetText();

        if (turn >= 2)//When the AI and the player have played
        {
            StartCombat();
            return;
        }

        if (!playerTurn)
        {
            AI.PlayAI();
        }
    }

    private void StartCombat()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Game)
        {
            SwitchCam(gameCamera, combatCamera);
            uIUtility.CanvasFadeOut(gameCanvaGroup);
        }

        turn = 0;
        attackManager.StartAttackTurn(playerTurn, () => EndCombat());
    }

    private void EndCombat()
    {
        if (GameManager.Instance.gameState == GameManager.GameState.Game)
        {
            SwitchCam(combatCamera, gameCamera);
            uIUtility.CanvasFadeIn(gameCanvaGroup);
        }

        EffectTurn?.Invoke();

        TraitManager.OnGridChange?.Invoke();

        if (!playerTurn)
        {
            AI.PlayAI();
        }
    }

    public bool GetPlayerTurn()
    {
        return playerTurn;
    }

    private void SetText()
    {
        textTurn.text = playerTurn ? "Player turn" : "AI turn";
        buttonTurn.interactable = playerTurn;
    }

    private void SwitchCam(GameObject actualCam, GameObject nextCam)
    {

        CinemachineCore.Instance.GetActiveBrain(0).m_DefaultBlend.m_Time = 0.25f;
        actualCam.SetActive(false);
        nextCam.SetActive(true);

    }
}
