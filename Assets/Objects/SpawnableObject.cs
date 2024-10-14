using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnableObject : NetworkBehaviour
{
    [SerializeField]
    private bool spawnsGrounded;

    [SerializeField] private Vector3 spawnBoundaries;

    public Vector3 getSpawnBoundaries()
    {
        return spawnBoundaries;
    }
    public bool spawnsOnGround()
    {
        return spawnsGrounded;
    }
}
