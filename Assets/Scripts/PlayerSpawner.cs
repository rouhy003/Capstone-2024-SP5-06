using Fusion;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public NetworkPrefabRef PCPlayerPrefab;
    private GameController _gameStateController = null;
    private bool isVR;
    public GameObject camera;
    public GameObject VRRig;
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
        if (!isVR)
        {
            Runner.Spawn(PCPlayerPrefab, new Vector3(0, 1, 0), Quaternion.identity);
        }
        else
        {
            camera.SetActive(false);
            VRRig.SetActive(true);
        }

    }
}
