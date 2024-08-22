using UnityEngine;
using Fusion;

public class PlayerObjectPush : NetworkBehaviour
{
    public float pushPower = 2f;

    void OnControllerColliderHit(UnityEngine.ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        
        // Requests the body's NetworkObject
        if (body.GetComponent<NetworkObject>() != null)
        {
            body.GetComponent<NetworkObject>().RequestStateAuthority();
        }

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
}
