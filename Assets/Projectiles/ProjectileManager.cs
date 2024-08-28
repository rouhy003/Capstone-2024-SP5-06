using Fusion;
using UnityEngine;
using System.Collections.Generic;


public class ProjectileManager : NetworkBehaviour
{
    [SerializeField] private List<NetworkObject> objectPool = new List<NetworkObject> ();
    [Networked] private int poolIndex { get; set; }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void FireProjectileRPC(Vector3 firePoint, Vector3 forward)
    { 
        var projectile = objectPool[poolIndex];
        Runner.SetIsSimulated(projectile, false);
        Runner.SetIsSimulated(projectile, true);
        projectile.GetComponent<PhysxBall>().Fire(firePoint, forward);
        poolIndex++;
        if (poolIndex >= objectPool.Count)
        {
            poolIndex = 0;
        }
    }
}
