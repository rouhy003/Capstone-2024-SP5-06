using Fusion;
using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : NetworkBehaviour
{
    public GameObject objectPrefab;

    private List<NetworkObject> currentObjects = new List<NetworkObject>();
    public const int ObjectLimit = 5;

    public override void FixedUpdateNetwork()
    {
        if (currentObjects.Count < ObjectLimit)
        {
            SpawnObject(Random.Range(5, -5), Random.Range(5, -5));
        }
    }
    public void SpawnObject(float x, float z)
    {
        Vector3 spawnPosition = new Vector3(x, 10, z);
        NetworkObject spawnedObject = Runner.Spawn(objectPrefab, spawnPosition);
        currentObjects.Add(spawnedObject);
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DespawnObject(NetworkObject spawnedObject)
    {
        Runner.Despawn(spawnedObject);
        currentObjects.Remove(spawnedObject);
    }
}
