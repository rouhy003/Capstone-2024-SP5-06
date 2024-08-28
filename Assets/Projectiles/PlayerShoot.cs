using Fusion;
using UnityEngine;

public class ProjectileShoot : NetworkBehaviour
{
    public float shootSpeed = 15f;

    public PlayerMovement PlayerMovement;
    ProjectileManager pm;

    void Start()
    {
        pm = FindObjectOfType<ProjectileManager>();
    }

    void Update()
    {
        if (HasStateAuthority == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = PlayerMovement.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            ray.origin += PlayerMovement.Camera.transform.forward;

            pm.FireProjectileRPC(ray.origin, PlayerMovement.Camera.transform.forward * shootSpeed);
           
        }
    }

}
