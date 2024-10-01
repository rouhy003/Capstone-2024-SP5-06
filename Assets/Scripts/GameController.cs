using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

public class GameController : NetworkBehaviour
{
    enum GamePhase
    {
        Starting,
        Running,
        Ending
    }

    private static GameController _singleton;
    [Networked] private GamePhase Phase { get; set; }
    [SerializeField] private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

    public static GameController Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton != null)
            {
                throw new InvalidOperationException();
            }
            _singleton = value;
        }
    }

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            Phase = GamePhase.Starting;
        }
    }

    public override void Render()
    {
        switch (Phase)
        {
            case GamePhase.Starting:
                Phase = GamePhase.Running;
                break;
            case GamePhase.Running:
                CheckForEndOfGame();
                break;
            case GamePhase.Ending:
                Runner.Shutdown();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void Awake()
    {
        GetComponent<NetworkObject>().Flags |= NetworkObjectFlags.MasterClientObject;
        Singleton = this;
    }

    private void OnDestroy()
    {
        if (Singleton == this)
        {
            _singleton = null;
        }
        else
        {
            throw new InvalidOperationException();
        }

    }

    public void TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
    {
        _playerDataNetworkedIds.Add(playerDataNetworkedId);
    }

    public void EndGame()
    {
        Phase = GamePhase.Ending;
    }

    public void CheckForEndOfGame()
    {
    }
}
