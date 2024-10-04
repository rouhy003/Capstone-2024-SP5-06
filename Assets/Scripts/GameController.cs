using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
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
    [Networked] TickTimer gameTimer { get; set; }
    [Networked] int playerCount { get; set; }
    [SerializeField] private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

    public TextMeshProUGUI timeUI;
    public int gameTime = 100;

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
        playerCount++;
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
                GameStarting();
                break;
            case GamePhase.Running:
                GameRunning();
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


    public void GameStarting()
    {
        if (playerCount == 1)
        {
            gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
            Phase = GamePhase.Running;
        }
    }
        
    public void GameRunning()
    {
        int time = (int)gameTimer.RemainingTime(Runner);
        timeUI.SetText(time.ToString());

        if (gameTimer.Expired(Runner))
        {
            Phase = GamePhase.Ending;
        }
    }
}
