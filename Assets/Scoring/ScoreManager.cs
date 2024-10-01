using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(ChangeP1Score))]
    [SerializeField] private int player1Score { get; set; }

    [Networked, OnChangedRender(nameof(ChangeP2Score))]
    [SerializeField] private int player2Score { get; set; }

    public TextMeshProUGUI p1score;
    public TextMeshProUGUI p2score;

    public void InitialiseGame()
    {
        if (HasStateAuthority)
        {
            player1Score = 0;
            player2Score = 0;
        }
    }

    public void ChangeP1Score()
    {
        Debug.Log(player1Score);
        p1score.SetText("Player 1 Score: " + player1Score.ToString());
    }

    public void ChangeP2Score()
    {
        Debug.Log(player2Score);
        p1score.SetText("Player 2 Score: " + player2Score.ToString());
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeP1ScoreRPC(bool increase, int amount)
    {
        if (increase)
        {
            player1Score += amount;
        }
        else
        {
            player1Score -= amount;
        }
        ChangeP1Score();
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeP2ScoreRPC(bool increase, int amount)
    {
        if (increase)
        {
            player2Score += amount;
        }
        else
        {
            player2Score -= amount;
        }
        ChangeP2Score();
    }
}
