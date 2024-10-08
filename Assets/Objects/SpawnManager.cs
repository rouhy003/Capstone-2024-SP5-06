using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] spawnPrefabs;

    // Represent's the object's size to calculate spawning space
    [SerializeField]
    protected Vector3 spawnBoundaries = new Vector3(0.3f, 0.5f, 0.3f);

    protected GameManager gm;

    protected List<NetworkObject> currentObjects = new List<NetworkObject>();

    [SerializeField]
    protected const int ObjectLimit = 10;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Attempts to spawn the specified prefab at the specified spawn position.
    public bool SpawnObject(GameObject prefab, Vector3 spawnPosition)
    {
        bool canSpawn = false;

        SpawnableObject prop = prefab.GetComponent<SpawnableObject>();
        if (prop != null)
        {
            // Grounded object spawning
            if (prop.spawnsOnGround())
            {
                RaycastHit groundCast;
                bool hasGround = Physics.Linecast(spawnPosition, spawnPosition + (10 * Vector3.down), out groundCast);

                if (hasGround && groundCast.collider.tag == "Ground")
                {
                    spawnPosition.Set(spawnPosition.x, groundCast.point.y + (spawnBoundaries.y / 2), spawnPosition.z);

                    // Checks to make sure that there is enough space to spawn the object
                    Collider[] overlap = Physics.OverlapBox(spawnPosition, new Vector3(spawnBoundaries.x, spawnBoundaries.y * 0.49f, spawnBoundaries.z));
                    if (overlap.Length < 1)
                    {
                        canSpawn = true;
                    }
                }
            }
            else
            {
                Collider[] overlap = Physics.OverlapSphere(spawnPosition, 0.5f);
                if (overlap.Length < 1)
                {
                    canSpawn = true;
                }
            }
        }

        // Checking to make sure that the object is within the perimeters of the room.
        Vector3[] directions = { Vector3.forward, Vector3.left, Vector3.right, Vector3.back };
        foreach (Vector3 vector in directions)
        {
            RaycastHit wallCast;
            LayerMask mask = LayerMask.GetMask("Wall");

            // If a wall isn't detected in at least one of the rays, then the object will not spawn.s
            if (!Physics.Raycast(spawnPosition, spawnPosition + vector , out wallCast, 20f, mask))
            {
                canSpawn = false;
            }
        }

        if (canSpawn)
        {
            NetworkObject spawnedObject = Runner.Spawn(prefab, spawnPosition);
            currentObjects.Add(spawnedObject);
            return true;
        }
        return false;
    }

    // Returns a random spawn position.
    protected Vector3 getRandomSpawnPosition()
    {
        return new Vector3(
                    Random.Range(20f, -20f),
                    Random.Range(1f, 5f),
                    Random.Range(20f, -20f));
    }

    // Despawns the specified object and removes it from the list of current objects.
    public void DespawnObject(NetworkObject spawnedObject)
    {
        Runner.Despawn(spawnedObject);
        currentObjects.Remove(spawnedObject);
    }
}
