using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveProjectile : PhysxBall
{
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private float explosionPower = 100f;

    void OnCollisionEnter(Collision collision)
    {
        Explode();
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
                prop.Knockdown();
            }
        }
        Runner.Despawn(Object);
    }
}
