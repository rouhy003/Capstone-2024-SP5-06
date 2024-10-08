using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class SpawnManager : NetworkBehaviour
{
    [SerializeField]
    public GameObject[] spawnPrefabs;

    [SerializeField]
    private Vector3 spawnBoundaries = new Vector3(0.3f, 0.5f, 0.3f);

    private GameManager gm;

    private List<NetworkObject> currentObjects = new List<NetworkObject>();
    public const int ObjectLimit = 10;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Attempts to spawn the specified prefab at the specified spawn position.
    public bool SpawnObject(GameObject prefab, Vector3 spawnPosition)
    {
        bool canSpawn = false;
        return canSpawn;
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
