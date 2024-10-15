using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BasicProjectileWeapon : GenericWeapon
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private AudioSource fireSound;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    //Overrides GenericWeapon Shoot method.
    //Spawns a projectile at the firePoint position and calls the fire method of the PhysxBall component to add velocity.
    //Also sets the player float in the PhysxBall script to the playerHolding float (to find out which player fired the projectile).
    public override void Shoot()
    {
        if (Runner != null)
        {
            NetworkObject proj = Runner.Spawn(projectile, FirePoint.position);
            PhysxBall p = proj.GetComponent<PhysxBall>();
            p.player = playerHolding;
            p.Fire(FirePoint.position, GetFiringRayOrigin() * shootSpeed);
            fireSound.Play();
        }
    }
}
