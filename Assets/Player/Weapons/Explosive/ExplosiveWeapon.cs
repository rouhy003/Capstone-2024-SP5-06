using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeapon : GenericWeapon
{
    [SerializeField] private GameObject projectile;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public override void Shoot()
    {
        Runner.Spawn(projectile, GetFiringRayOrigin());
    }
}
