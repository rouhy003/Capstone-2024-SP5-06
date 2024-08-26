using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PhysxBall : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }
    

    public void Init(Vector3 forward)
    {
        life = TickTimer.CreateFromSeconds(Runner, 5.0f);
        GetComponent<Rigidbody>().velocity = forward;
        FindObjectOfType<ProjectileManager>().RpcSetStateAuthority(gameObject.GetComponent<NetworkObject>());
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
    }

    /*
    public void OnCollisionEnter(Collision collision)
    {
        RequestAllStateAuthroity(collision.gameObject.transform.position, 2f);
    }
    */

    // Used to request the state authority of all objects within a specified radius to make sure physics of nearby objects functions together properly
    // Not an efficient solution, but it somewhat works...
    /*
    public void RequestAllStateAuthroity(Vector3 centre, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(centre, radius);
        foreach (var hitCollider in hitColliders)
        {
            try
            {
                NetworkObject networkObject = hitCollider.gameObject.GetComponent<NetworkObject>();
                if (networkObject != null) if (!networkObject.HasStateAuthority) networkObject.RequestStateAuthority();
            }
            catch { }
        }
    }
    */
}
