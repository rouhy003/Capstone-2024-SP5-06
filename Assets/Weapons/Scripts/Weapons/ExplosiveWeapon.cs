using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

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
        NetworkObject proj = Runner.Spawn(projectile, GetFiringRayOrigin());
        proj.GetComponent<PhysxBall>().player = playerHolding;
    }
}
