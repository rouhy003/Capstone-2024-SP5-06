using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnableObject : NetworkBehaviour
{
    [SerializeField]
    private bool spawnsGrounded;

    public bool spawnsOnGround()
    {
        return spawnsGrounded;
    }
}
