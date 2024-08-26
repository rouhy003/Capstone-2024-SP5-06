using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

public class PlayerObjectPush : NetworkBehaviour
{
    public float pushPower = 2f;
    public float overlapRadius = 3f;

    void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit)
    {
        //RequestAllStateAuthroity(gameObject.transform.position, 3f);

        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;
    }

    // Used to request the state authority of all objects within a specified radius to make sure physics of nearby objects functions together properly
    // Not an efficient solution, but it somewhat works...
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
}
