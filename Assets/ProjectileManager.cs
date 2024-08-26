using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : NetworkBehaviour
{
    // Should only run by the player who has state authroity over this object (whoever joins first in a shared game).
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcSetStateAuthority(NetworkObject networkObject)
    {
        try
        {
            if (!networkObject.HasStateAuthority)
            {
                networkObject.RequestStateAuthority();
                print("This object is now mine!");
            }
        }
        catch
        {

        }
    }
}
