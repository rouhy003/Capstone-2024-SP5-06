using Fusion;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObjectManager : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] objectPrefabs;

    // Represents the object's size to calculate spawning space
    // Currently set to a default value, but it should be calculated based on the prefab's collision
    private Vector3 objectSize = new Vector3(0.3f, 0.5f, 0.3f);

    private List<NetworkObject> currentObjects = new List<NetworkObject>();
    public const int ObjectLimit = 10;

    public override void FixedUpdateNetwork()
    {
        // Attempts to spawn an object if the limit has yet to be reached.
        if (currentObjects.Count < ObjectLimit)
        {
            Vector3 spawnPosition = getRandomSpawnPosition();

            bool objectSpawned = false;
            int spawnAttemptsRemaining = 10;

            // Repeats the spawn attempt until either the object spawns successfully or there are no more remaining attempts.
            while (!objectSpawned && spawnAttemptsRemaining > 0)
            {
                spawnPosition = getRandomSpawnPosition();
                objectSpawned = SpawnObject(objectPrefabs[Random.Range(0, objectPrefabs.Length)], spawnPosition);
                spawnAttemptsRemaining--;
            }
        }
    }

    // Attempts to spawn an object at "x" and "y".
    public bool SpawnObject(GameObject prefab, Vector3 spawnPosition)
    {
        Debug.Log(spawnPosition);

        bool canSpawn = false;

        PropObject prop = prefab.GetComponent<PropObject>();
        if (prop != null)
        {
            // Grounded object spawning
            if (prop.spawnsOnGround())
            {
                RaycastHit groundCast;
                bool hasGround = Physics.Linecast(spawnPosition, spawnPosition + (10 * Vector3.down), out groundCast);

                if (hasGround && groundCast.collider.tag == "Ground")
                {
                    spawnPosition.Set(spawnPosition.x, groundCast.point.y + (objectSize.y / 2), spawnPosition.z);

                    // Checks to make sure that there is enough space to spawn the object
                    Collider[] overlap = Physics.OverlapBox(spawnPosition, new Vector3(objectSize.x, objectSize.y * 0.49f, objectSize.z));
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

        Physics.Raycast(spawnPosition, Vector3.forward, 50f);


        if (canSpawn)
        {
            NetworkObject spawnedObject = Runner.Spawn(prefab, spawnPosition);
            currentObjects.Add(spawnedObject);
            return true;
        }
        return false;
    }

    // Returns a random spawn position.
    private Vector3 getRandomSpawnPosition()
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
