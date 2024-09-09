using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveProjectile : PhysxBall
{
    [SerializeField] private float explosionRadius = 10f;
    [SerializeField] private float explosionPower = 30f;

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
                    explosionRadius,
                    upwardsModifier: 3f
                );
            }
        }
        Runner.Despawn(Object);
    }
}
