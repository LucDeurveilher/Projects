using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public AIBase AI;

    static public Action EffectTurn;

    private void Start()
    {
        GameManager.Instance.PlayerTurn = playerTurn;
        RandomStarter();
    }

    private void Update()
    {
        if (buttonDraw.interactable != !GameManager.Instance.CardPlayed)
            buttonDraw.interactable = !GameManager.Instance.CardPlayed;


    }

    void RandomStarter()
    {
        int starter = Random.Range(0, 2);
        Debug.Log($"Random = {starter}");

        starter = 0;//always player/AI

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
        turn = 0;
        attackManager.StartAttackTurn(playerTurn,() => EndCombat());
    }

    private void EndCombat()
    {
        EffectTurn?.Invoke();

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
}
