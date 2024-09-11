using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class PropObject : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }
    [SerializeField] private const int objectLifetime = 3;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, objectLifetime);
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Despawn();
        }
    }

    // Despawns the object through the current scene's object manager.
    public void Despawn()
    {
        FindObjectOfType<ObjectManager>().DespawnObject(Object);
    }
}
