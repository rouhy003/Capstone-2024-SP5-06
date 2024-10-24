using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AutoDespawn : NetworkBehaviour
{
    private SpawnableObject spawnedObject;

    [Networked] TickTimer life { get; set; }
    [SerializeField] private float objectLifetime;

    private void Start()
    {
        spawnedObject = GetComponent<SpawnableObject>();
    }
    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, objectLifetime);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            // Despawns the object
            // If the object is a spawnable one, it calls the script's own despawn method.
            if (spawnedObject != null) spawnedObject.Despawn();
            else Runner.Despawn(Object);
        }
    }
}
