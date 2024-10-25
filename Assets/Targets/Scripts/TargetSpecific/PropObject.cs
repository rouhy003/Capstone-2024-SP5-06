using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class PropObject : NetworkBehaviour
{
    private PropSound propSound;
    private PropState propState;
    private ParticleSpawner particleSpawner;

    [Networked] TickTimer life { get; set; }
    [SerializeField] private float objectLifetime = 3;
    [SerializeField] private float knockdownTime = 0;
    private bool hasBeenStruck = false;

    [SerializeField] private int pointValue = 1;
    [SerializeField] private bool addPoints = true;

    public ScoreManager sm;

    public override void Spawned()
    {
        life = TickTimer.CreateFromSeconds(Runner, objectLifetime);
        sm = FindObjectOfType<ScoreManager>();
        propSound = GetComponent<PropSound>();
        propState = GetComponent<PropState>();
        particleSpawner = GetComponent<ParticleSpawner>();
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
        // Can only be ran through once per object
        if (!hasBeenStruck)
        {
            // Adds points to owner of the projectile who knocked down the object.
            if (player == 1)
            {
                sm.ChangeP1ScoreRPC(addPoints, pointValue);
            }
            else if (player == 2)
            {
                sm.ChangeP2ScoreRPC(addPoints, pointValue);
            }

            // Plays a hit sound, if it has one.
            if (propSound != null) propSound.PlayHitSoundRPC();

            // Changes the prop's mesh, if it changes state.
            if (propState != null) propState.UseDamageMesh();

            // Sets a timer for the object's remaining lifetime.
            // Have "knockdownTime" set to 0 to have objects despawn immediately upon impact.
            life = TickTimer.CreateFromSeconds(Runner, knockdownTime);

            hasBeenStruck = true;
        }
    }

    // Despawns the object through the current scene's object manager.
    private void Despawn()
    {
        // Spawns a particle on despawn, if it has one
        if (particleSpawner != null && hasBeenStruck) particleSpawner.SpawnParticleRPC(transform.position);

        FindObjectOfType<ObjectManager>().DespawnObject(Object);
    }
}
