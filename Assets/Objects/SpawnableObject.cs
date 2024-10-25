using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnableObject : NetworkBehaviour
{
    private SpawnManager spawnManager;

    // Determines whether the object will spawn anchored to the ground.
    [SerializeField] private bool spawnsGrounded;

    // Represents the spawning chance of the object
    // Should be set from 0 to 1, where 1 means that the object will always spawn on every attempt.
    [SerializeField] private float spawnChance;

    // Represents the volume of space that needs to be empty for the object to spawn.
    [SerializeField] private Vector3 spawnBoundaries;
    
    public void SetSpawnManager(SpawnManager setManager) { spawnManager = setManager; }

    public Vector3 getSpawnBoundaries() { return spawnBoundaries; }
    public bool spawnsOnGround() { return spawnsGrounded; }
    public float getSpawnChance() { return spawnChance; }

    // Calls the referenced spawn manager to despawn this current object.
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DespawnRPC() { spawnManager.DespawnObject(Object); }
}
