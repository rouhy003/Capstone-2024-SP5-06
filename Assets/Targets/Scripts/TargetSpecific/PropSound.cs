using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PropSound : NetworkBehaviour
{
    private AudioSource hitSound;

    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void PlayHitSoundRPC()
    {
        hitSound.Play();
    }
}
