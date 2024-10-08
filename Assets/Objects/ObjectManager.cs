using Fusion;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObjectManager : SpawnManager
{
    [SerializeField]
    public GameObject[] objectPrefabs;

    private ScoreManager sm;

    void Start()
    {
        sm = FindObjectOfType<ScoreManager>();
    }

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
    public new bool SpawnObject(GameObject prefab, Vector3 spawnPosition)
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

        Physics.Raycast(spawnPosition, Vector3.forward, 50f);


        if (canSpawn)
        {
            NetworkObject spawnedObject = Runner.Spawn(prefab, spawnPosition);
            spawnedObject.GetComponent<PropObject>().sm = sm;
            currentObjects.Add(spawnedObject);
            return true;
        }
        return false;
    }
}
