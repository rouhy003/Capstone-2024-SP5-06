using Fusion;
using UnityEngine;

public class ProjectileShoot : NetworkBehaviour
{
    public GameObject projectile;
    public float shootSpeed = 1f;

    public PlayerMovement PlayerMovement;

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += PlayerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Runner.Spawn(projectile, ray.origin, Quaternion.identity);
        }
    }

}
