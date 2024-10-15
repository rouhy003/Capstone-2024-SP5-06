using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class PropObject : NetworkBehaviour
{
    private bool hasBeenStruck = false;

    [Networked] TickTimer life { get; set; }

    [SerializeField] private float objectLifetime = 3;
    [SerializeField] private float knockdownTime = 0;

    [SerializeField] private int pointValue = 1;
    [SerializeField] private bool addPoints = true;

    public ScoreManager sm;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, objectLifetime);
        sm = FindObjectOfType<ScoreManager>();
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Despawn();
        }
    }

    // Executes whenever the object is struck by a projectile.
    public void Knockdown(int player)
    {
        // Resets the object's life if not previously struck before.
        if (!hasBeenStruck)
        {
            if (player == 1)
            {
                sm.ChangeP1ScoreRPC(addPoints, pointValue);
            }
            else if (player == 2)
            {
                sm.ChangeP2ScoreRPC(addPoints, pointValue);
            }
            life = TickTimer.CreateFromSeconds(Runner, knockdownTime);
        }
    }

    // Despawns the object through the current scene's object manager.
    private void Despawn()
    {
        FindObjectOfType<ObjectManager>().DespawnObject(Object);
    }
}
