using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PhysxBall : NetworkBehaviour
{
    private NetworkRigidbody3D _rigidbody;

    protected AudioSource audioSource;

    // Audio and visual effects
    [SerializeField] protected AudioClip m_bounceSound;
    [SerializeField] protected GameObject bounceParticle;

    [Networked] TickTimer life { get; set; }
    [SerializeField] private int lifetime = 3;

    [SerializeField] private bool despawnOnCollision = false;

    public int player;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<NetworkRigidbody3D>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public override void FixedUpdateNetwork()
    {
        // Despawns the projectile once its lifetime runs out.
        if (life.Expired(Runner)) Despawn();
    }

    // Fires the projectile
    public void Fire(Vector3 firePoint, Vector3 forward)
    {
        //_rigidbody.Teleport(firePoint, Quaternion.identity);

        _rigidbody.Rigidbody.isKinematic = false;
        _rigidbody.Rigidbody.AddForce(forward, ForceMode.Impulse);

        life = TickTimer.CreateFromSeconds(Runner, lifetime);
    }

    // Despawns the projectile
    public virtual void Despawn()
    {
        Runner.Despawn(Object);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the projectile collides with a prop, it gets knocked down.
        PropObject prop = collision.gameObject.GetComponent<PropObject>();
        if (prop != null)
        {
            prop.Knockdown(player);
        }

        // Plays the sound effect, if it exists.
        if (m_bounceSound != null)
        {
            audioSource.clip = m_bounceSound;
            audioSource.Play();
        }

        // Plays the particle effect, if it exists.
        if (bounceParticle != null) Runner.Spawn(bounceParticle, collision.collider.transform.position);

        if (despawnOnCollision) Despawn();
    }
}
