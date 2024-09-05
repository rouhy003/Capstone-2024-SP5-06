using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericWeapon : NetworkBehaviour
{
    protected float shootSpeed = 10f;

    private PlayerMovement PlayerMovement;

    protected void Start()
    {
        PlayerMovement = gameObject.GetComponentInParent<PlayerMovement>();
    }

    // Returns the origin point of where the projectile will fire from
    protected virtual Vector3 GetFiringRayOrigin()
    {
        Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin += GetCameraTransformForward();
        return ray.origin;
    }

    // Returns the forward vector of where the projectile will fire from
    protected virtual Vector3 GetCameraTransformForward()
    {
        return PlayerMovement.Camera.transform.forward;
    }

    public abstract void Shoot();
}
