using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class SpawnManager : NetworkBehaviour
{
    // Represents the current scene's game manager (if it exists).
    protected GameManager gameManager;

    [SerializeField]
    public GameObject[] spawnPrefabs;

    protected List<NetworkObject> currentObjects = new List<NetworkObject>();

    [SerializeField]
    protected int ObjectLimit = 10;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Called every fixed update.
    public override void FixedUpdateNetwork()
    {
        // Attempts to spawn an object if the limit has yet to be reached.
        if (currentObjects.Count < ObjectLimit && gameManager.Phase == GameManager.GamePhase.Running)
        {
            Vector3 spawnPosition = getRandomSpawnPosition();

            bool objectSpawned = false;
            int spawnAttemptsRemaining = 20;

            // Repeats the spawn attempt until either the object spawns successfully or there are no more remaining attempts.
            while (!objectSpawned && spawnAttemptsRemaining > 0)
            {
                spawnPosition = getRandomSpawnPosition();
                objectSpawned = SpawnObject(spawnPrefabs[Random.Range(0, spawnPrefabs.Length)], spawnPosition);
                spawnAttemptsRemaining--;
            }
        }
    }

    // Attempts to spawn the specified prefab at the specified spawn position.
    public bool SpawnObject(GameObject prefab, Vector3 spawnPosition)
    {
        bool canSpawn = false;

        SpawnableObject prop = prefab.GetComponent<SpawnableObject>();
        if (prop != null)
        {
            // Determines whether the object will spawn in this current attempt or not, based on the object's spawnChance attribute.
            // Objects will always spawn if their spawnChance attribute is set to 1, for example.
            if (Random.Range(0f, 1f) <= prop.getSpawnChance())
            {
                Vector3 spawnBoundaries = prop.getSpawnBoundaries();
                Debug.Log(spawnBoundaries + " " + gameObject.name);

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
                    // Checks to make sure that there is enough space to spawn the object
                    // Uses the "X" value of the "spawnBoundaries" attribute.
                    Collider[] overlap = Physics.OverlapSphere(spawnPosition, spawnBoundaries.x);
                    if (overlap.Length < 1)
                    {
                        canSpawn = true;
                    }
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
            NetworkObject spawnedObject = Runner.Spawn(prefab, spawnPosition, getRandomRotation());
            spawnedObject.GetComponent<SpawnableObject>().SetSpawnManager(this);
            currentObjects.Add(spawnedObject);
            return true;
        }
        return false;
    }

    // Returns a random spawn position.
    protected Vector3 getRandomSpawnPosition()
    {
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();
        var randomPosition = room.GenerateRandomPositionInRoom(0.5f, true);

        if (randomPosition.HasValue) return randomPosition.Value;
        else return new Vector3(
            Random.Range(20f, -20f),
            Random.Range(1f, 5f),
            Random.Range(20f, -20f));
    }

    protected Quaternion getRandomRotation()
    {
        return new Quaternion(
            0f,
            Random.rotation.y,
            0f,
            Random.rotation.w);
    }

    // Despawns the specified object and removes it from the list of current objects.
    public void DespawnObject(NetworkObject spawnedObject)
    {
        Runner.Despawn(spawnedObject);
        currentObjects.Remove(spawnedObject);
    }
}
