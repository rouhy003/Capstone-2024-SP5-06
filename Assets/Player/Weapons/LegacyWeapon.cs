using Fusion;
using UnityEngine;

public class LegacyWeapon : NetworkBehaviour
{
    public float shootSpeed = 15f;

    public PlayerMovement PlayerMovement;
    ProjectileManager pm;

    void Start()
    {
        pm = FindObjectOfType<ProjectileManager>();
    }

    public void Shoot()
    {
        Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin += PlayerMovement.Camera.transform.forward;

        pm.FireProjectileRPC(ray.origin, PlayerMovement.Camera.transform.forward * shootSpeed);
    }
}
