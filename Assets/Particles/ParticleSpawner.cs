using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ParticleSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    // Spawns a particle prefab at the specified location.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void SpawnParticleRPC(Vector3 location)
    {
        Runner.Spawn(particlePrefab, location);
    }
}
