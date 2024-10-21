using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnableObject : NetworkBehaviour
{
    [SerializeField] private bool spawnsGrounded;

    // Represents the spawning chance of the object
    // Should be set from 0 to 1, where 1 means that the object will always spawn on every attempt.
    [SerializeField] private float spawnChance;

    [SerializeField] private Vector3 spawnBoundaries;

    public Vector3 getSpawnBoundaries()
    {
        return spawnBoundaries;
    }
    public bool spawnsOnGround()
    {
        return spawnsGrounded;
    }
    public float getSpawnChance()
    {
        return spawnChance;
    }
}
