using Fusion;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ObjectManager : NetworkBehaviour
{
    public GameObject objectPrefab;

    // Represents the object's size to calculate spawning space
    // Currently set to a default value, but it should be calculated based on the prefab's collision
    private Vector3 objectSize = new Vector3(0.3f, 0.5f, 0.3f);

    private List<NetworkObject> currentObjects = new List<NetworkObject>();
    public const int ObjectLimit = 100;

    public override void FixedUpdateNetwork()
    {
        if (currentObjects.Count < ObjectLimit)
        {
            SpawnObject(Random.Range(10, -10), Random.Range(10, -10));
        }
    }

    // Attempts to spawn an object at "x" and "y".
    public void SpawnObject(float x, float z)
    {
        // Sets the object spawn position at set elevation, then re-elevates the position based on whether there is ground underneath
        Vector3 spawnPosition = new Vector3(x, 5, z);
        float groundElevation = GetGroundElevation(spawnPosition);

        // If there is ground underneath where the object can spawn
        if (groundElevation != float.NegativeInfinity)
        {
            // Updates the spawn position to spawn on the ground
            spawnPosition.Set(spawnPosition.x, groundElevation + (objectSize.y / 2), spawnPosition.z);

            Debug.Log(objectSize);

            // Checks to make sure that there is enough space to spawn the object
            Collider[] overlap = Physics.OverlapBox(spawnPosition, new Vector3(objectSize.x, objectSize.y * 0.49f, objectSize.z));
            if (overlap.Length < 1)
            {
                NetworkObject spawnedObject = Runner.Spawn(objectPrefab, spawnPosition);
                currentObjects.Add(spawnedObject);
            }
        }
    }

    // Returns the elevation of the ground underneath the spawn position, if detected.
    public float GetGroundElevation(Vector3 spawnPosition)
    {
        RaycastHit hit;

        // Shifts the spawn position's Y value to match the elevation of the ground, if found.
        if (Physics.Linecast(spawnPosition, spawnPosition + ( 10 * Vector3.down), out hit)) {
            return hit.point.y;
        }
        else
        {
            // Returns negative infinity if there is no ground underneath
            return float.NegativeInfinity;
        }
    }

    // Despawns the specified object and removes it from the list of current objects.
    public void DespawnObject(NetworkObject spawnedObject)
    {
        Runner.Despawn(spawnedObject);
        currentObjects.Remove(spawnedObject);
    }
}
