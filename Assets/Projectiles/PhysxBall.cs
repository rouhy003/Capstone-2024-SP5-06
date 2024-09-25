using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PhysxBall : NetworkBehaviour
{
    private NetworkRigidbody3D _rigidbody;
    [SerializeField]

    [Networked] TickTimer life { get; set; }
    [SerializeField] private const int lifetime = 3;

    protected void Awake()
    {
        _rigidbody = GetComponent<NetworkRigidbody3D>();
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Despawn();
        }
    }

    public void Fire(Vector3 firePoint, Vector3 forward)
    {
        //_rigidbody.Teleport(firePoint, Quaternion.identity);

        _rigidbody.Rigidbody.isKinematic = false;
        _rigidbody.Rigidbody.AddForce(forward, ForceMode.Impulse);

        life = TickTimer.CreateFromSeconds(Runner, lifetime);
    }

    public void Despawn()
    {
        Runner.Despawn(Object);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PropObject prop = collision.gameObject.GetComponent<PropObject>();
        if (prop != null)
        {
            prop.Knockdown();
        }
    }
}
