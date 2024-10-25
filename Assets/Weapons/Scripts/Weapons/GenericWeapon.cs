using Fusion;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Base class for all possible weapons. Contains a Shoot method that needs to be implemented for each weapon
//and WeaponCoolDown coroutine for the cooldown.
public abstract class GenericWeapon : NetworkBehaviour
{
    [SerializeField] protected float shootSpeed = 10f;
    protected PlayerMovement PlayerMovement;
    
    public Transform FirePoint;
    public float CoolDownTime = 2;
    public bool isVR = false;
    public int playerHolding;
    
    private bool coroutineRunning = false;
    private bool canShoot = true;
   
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

    public bool GetCanShoot()
    {
        return canShoot;
    }

    public IEnumerator WeaponCoolDown()
    {
        if (!coroutineRunning)
        {
            coroutineRunning = true;
            canShoot = false;
            yield return new WaitForSeconds(CoolDownTime);
            canShoot = true;
            coroutineRunning = false;
        }
    }
}
