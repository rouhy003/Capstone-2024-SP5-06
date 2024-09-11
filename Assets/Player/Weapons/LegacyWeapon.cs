using Fusion;
using UnityEngine;

public class LegacyWeapon : GenericWeapon
{
    ProjectileManager pm;

    new private void Start()
    {
        base.Start();
        pm = FindObjectOfType<ProjectileManager>();
    }

    public override void Shoot()
    {
        pm.FireProjectileRPC(GetFiringRayOrigin(), GetCameraTransformForward() * shootSpeed);
    }
}
