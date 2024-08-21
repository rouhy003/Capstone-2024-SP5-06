using Fusion;
using UnityEngine;

public class ProjectileShoot : NetworkBehaviour
{
    public GameObject projectile;
    private float shootSpeed = 5f;

    public PlayerMovement PlayerMovement;

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        ray.origin += PlayerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Runner.Spawn(projectile,
                ray.origin,
                Quaternion.identity,
                Object.InputAuthority,
                (runner, o) =>
                {
                    // Calls the projectile's "Init()" method to set its velocity.
                    o.GetComponent<PhysxBall>().Init(PlayerMovement.Camera.transform.forward * shootSpeed);
                });
        }
    }

}
