using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SoundManager : NetworkBehaviour
{
    private AudioSource m_audioSource;

    [SerializeField] private AudioClip[] m_fanfares;
    [SerializeField] private AudioClip[] m_scoreSounds;

    // Used to limit the end fanfare method to only play once
    // because the GameManager's GameEnding() method keeps looping.
    private bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponentInChildren<AudioSource>();
    }

    // Plays a positive or negative sound effect depending on the result.
    private void PlayPositiveOrNegativeSound(bool isPositiveResult, AudioClip m_positive, AudioClip m_negative)
    {
        if (isPositiveResult) m_audioSource.clip = m_positive;
        else m_audioSource.clip = m_negative;
        m_audioSource.Play();
    }

    // Plays the scoring sound. Selects different clips depending on if the player gained or lost points.
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void PlayScoreSoundRPC(bool isPositive)
    {
        PlayPositiveOrNegativeSound(isPositive, m_scoreSounds[0], m_scoreSounds[1]);
    }

    public void PlayStartFanfare()
    {
        m_audioSource.clip = m_fanfares[0];
        m_audioSource.Play();
    }

    // Plays the end fanfare to the game. Selects different clips depending on if the player won or lost.
    public void PlayEndFanfare(bool playerWon)
    {
        if (!isGameOver)
        {
            PlayPositiveOrNegativeSound(playerWon, m_fanfares[1], m_fanfares[2]);
            isGameOver = true;
        }
    }
}
