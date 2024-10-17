using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

public class PhysxBall : NetworkBehaviour
{
    private NetworkRigidbody3D _rigidbody;

    protected AudioSource audioSource;
    [SerializeField] protected AudioClip m_bounceSound;

    [Networked] TickTimer life { get; set; }
    [SerializeField] private int lifetime = 3;

    [SerializeField] private bool despawnOnCollision = false;

    public int player;

    protected void Awake()
    {
        _rigidbody = GetComponent<NetworkRigidbody3D>();
        audioSource = GetComponentInChildren<AudioSource>();
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

    public virtual void Despawn()
    {
        Runner.Despawn(Object);
    }

    private void OnCollisionEnter(Collision collision)
    {
        PropObject prop = collision.gameObject.GetComponent<PropObject>();
        if (prop != null)
        {
            prop.Knockdown(player);
        }

        PlayCollisionSound();

        if (despawnOnCollision)
        {
            Despawn();
        }
    }

    // Plays the sound effect upon colliding with a surface, if that sound effect exists.
    private void PlayCollisionSound()
    {
        if (!despawnOnCollision && m_bounceSound != null)
        {
            audioSource.clip = m_bounceSound;
            audioSource.Play();
        }
    }
}
