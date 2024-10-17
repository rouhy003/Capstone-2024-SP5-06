using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveProjectile : PhysxBall
{
    [SerializeField] private GameObject explosionPrefab;

    private float explosionRadius = 1.25f;
    private float explosionPower = 20f;

    protected override void Awake()
    {
        base.Awake();
    }

    // Explodes the projectile
    public void Explode()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, explosionRadius);

        // Applies force to each rigidbody within the explosion radius
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(
                    explosionPower,
                    explosionPosition,
                    explosionRadius
                );
            }

            PropObject prop = collider.gameObject.GetComponent<PropObject>();
            if (prop != null)
            {
                prop.Knockdown(player);
            }
            Runner.Spawn(explosionPrefab, transform.position);
        }
    }

    public override void Despawn()
    {
        Explode();
        base.Despawn();
    }
}
