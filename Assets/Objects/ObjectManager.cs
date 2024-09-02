using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : NetworkBehaviour
{
    public GameObject objectPrefab;

    [SerializeField] private List<NetworkObject> currentObjects { get; set; }
    public const int ObjectLimit = 5;

    public override void FixedUpdateNetwork()
    {
        // TODO: Insert spawning functionality here.
        if (currentObjects.Count < ObjectLimit)
        {
            SpawnObject(Random.Range(10, -10), Random.Range(10, -10));
        }
        Debug.Log(currentObjects.Count);
    }
    public void SpawnObject(float x, float z)
    {
        Vector3 spawnPosition = new Vector3(x, 5f, z);
        NetworkObject spawnedObject = Runner.Spawn(objectPrefab, spawnPosition);
        if (spawnedObject != null) currentObjects.Add(spawnedObject);
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DespawnObject(NetworkObject spawnedObject)
    {
        Runner.Despawn(spawnedObject);
    }
}
