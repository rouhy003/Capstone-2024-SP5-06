using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;

public class ScoreManager : NetworkBehaviour
{
    private AudioSource m_scoreManagerAudio;
    [SerializeField] private AudioClip m_startGame;
    [SerializeField] private AudioClip m_scorePositive;
    [SerializeField] private AudioClip m_scoreNegative;

    [Networked, OnChangedRender(nameof(ChangeP1Score))]
    [SerializeField] private int player1Score { get; set; }

    [Networked, OnChangedRender(nameof(ChangeP2Score))]
    [SerializeField] private int player2Score { get; set; }

    public TextMeshProUGUI p1score;
    public TextMeshProUGUI p2score;

    void Start()
    {
        m_scoreManagerAudio = GetComponentInChildren<AudioSource>();
    }

    //Sets the initial score of both players
    public void InitialiseGame()
    {
        if (HasStateAuthority)
        {
            player1Score = 0;
            player2Score = 0;
        }
        m_scoreManagerAudio.clip = m_startGame;
        m_scoreManagerAudio.Play();
    }

    //Updates the UI text for player 1s score.
    public void ChangeP1Score()
    {
        p1score.SetText("Player 1 Score: " + player1Score.ToString());
    }

    //Updates the UI text for player 2s score.
    public void ChangeP2Score()
    {
        p2score.SetText("Player 2 Score: " + player2Score.ToString());
    }

    //RPC for increasing player 1s score. If increase is set to true the score goes up by the set amount
    //if its false the score goes down.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeP1ScoreRPC(bool increase, int amount)
    {
        if (increase)
        {
            player1Score += amount;
            m_scoreManagerAudio.clip = m_scorePositive;
            m_scoreManagerAudio.Play();
        }
        else
        {
            player1Score -= amount;
            m_scoreManagerAudio.clip = m_scoreNegative;
            m_scoreManagerAudio.Play();
        }
        ChangeP1Score();
    }

    //RPC for increasing player 2s score. If increase is set to true the score goes up by the set amount
    //if its false the score goes down.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeP2ScoreRPC(bool increase, int amount)
    {
        if (increase)
        {
            player2Score += amount;
            m_scoreManagerAudio.clip = m_scorePositive;
            m_scoreManagerAudio.Play();
        }
        else
        {
            player2Score -= amount;
            m_scoreManagerAudio.clip = m_scoreNegative;
            m_scoreManagerAudio.Play();
        }
        ChangeP2Score();
    }

    //Returns player 1 score.
    public int GetP1Score()
    {
        return player1Score;
    }

    //Returns player 2 score.
    public int GetP2Score()
    {
        return player2Score;
    }
}
