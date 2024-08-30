using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkPrefabRef PCPlayerPrefab;
    public NetworkPrefabRef VRPlayerPrefab;
    public Canvas PlayerUI;
    private GameController _gameStateController = null;
    private bool isVR;
    [Networked] private bool _gameIsReady { get; set; } = false;


    public override void Spawned()
    {
        isVR = FindObjectOfType<LoadPCorVR>().isVR;
        if (_gameIsReady)
        {
            SpawnPlayer(Runner.LocalPlayer);
        }
    }

    public void StartPlayerSpawner(GameController gameController)
    {
        _gameIsReady = true;
        _gameStateController = gameController;
        RpcInitialPlayerSpawn();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcInitialPlayerSpawn()
    {
        SpawnPlayer(Runner.LocalPlayer);
    }

    public void SpawnPlayer(PlayerRef player)
    {
        NetworkObject p;
        if (isVR)
        {
            p = Runner.Spawn(VRPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
        else
        {
            p = Runner.Spawn(PCPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
        if (PlayerUI != null)
        {
            Canvas c = Instantiate(PlayerUI);
            c.transform.SetParent(p.transform, false);
        }
    }
}
