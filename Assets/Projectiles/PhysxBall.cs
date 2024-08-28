using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PhysxBall : NetworkBehaviour
{
    private NetworkRigidbody3D _rigidbody;
    [SerializeField]

    protected void Awake()
    {
        _rigidbody = GetComponent<NetworkRigidbody3D>();
    }

    public void Fire(Vector3 firePoint, Vector3 forward)
    {
        _rigidbody.Teleport(firePoint, Quaternion.identity);

        _rigidbody.Rigidbody.isKinematic = false;
        _rigidbody.Rigidbody.AddForce(forward, ForceMode.Impulse);
    }
}
