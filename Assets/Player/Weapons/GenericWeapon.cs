using Fusion;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GenericWeapon : NetworkBehaviour
{
    [SerializeField] protected float shootSpeed = 10f;

    protected PlayerMovement PlayerMovement;
    public Transform FirePoint;
    public bool isVR = false;

    protected void Start()
    {
        if (!isVR)
        {
            PlayerMovement = gameObject.GetComponentInParent<PlayerMovement>();
        }
    }

    // Returns the origin point of where the projectile will fire from
    protected virtual Vector3 GetFiringRayOrigin()
    {
        if (!isVR)
        {
            Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin += GetCameraTransformForward();
            return ray.origin;
        }
        else
        {
            return FirePoint.forward;
        }
    }

    // Returns the forward vector of where the projectile will fire from
    protected virtual Vector3 GetCameraTransformForward()
    {
        return PlayerMovement.Camera.transform.forward;
    }

    // Represents the firing action of the weapon
    public abstract void Shoot();
}
