using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ParticleSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    // Spawns a particle prefab at the specified location.
    public void SpawnParticle(Vector3 location)
    {
        Runner.Spawn(particlePrefab, location);
    }
}
