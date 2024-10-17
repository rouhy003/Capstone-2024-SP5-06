using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AutoDespawn : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }
    private float objectLifetime = 1;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, objectLifetime);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }
}
