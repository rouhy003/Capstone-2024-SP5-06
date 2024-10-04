using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class BasicProjectileWeapon : GenericWeapon
{
    [SerializeField] private GameObject projectile;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    public override void Shoot()
    {
        if (Runner != null)
        {
            NetworkObject proj = Runner.Spawn(projectile, FirePoint.position);
            PhysxBall p = proj.GetComponent<PhysxBall>();
            p.player = playerHolding;
            p.Fire(FirePoint.position, GetFiringRayOrigin() * shootSpeed);
        }
    }
}
