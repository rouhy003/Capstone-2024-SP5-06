using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public enum GamePhase
    {
        PreGame,
        Starting,
        Running,
        Ending
    }

    private static GameManager _singleton;
    [Networked] public GamePhase Phase { get; set; }
    [Networked] TickTimer gameTimer { get; set; }
    [Networked] int playerCount { get; set; }
    [SerializeField] private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();
    UIManager[] uiList;

    public int gameTime = 100;
    public int preGameTime = 10;
    public int postGameTime = 15;

    ScoreManager sm;
    SoundManager soundManager;

    private void Start()
    {
        soundManager = GetComponentInChildren<SoundManager>();
    }

    //Constrocts the GameManager as a singleton.
    public static GameManager Singleton
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

    //Called after object is spawned. Increases player count 
    public override void Spawned()
    {
        playerCount++;
        sm = FindObjectOfType<ScoreManager>();
        uiList = FindObjectsOfType<UIManager>();
        MasterController[] a = FindObjectsOfType<MasterController>();
        if (playerCount == 1)
        {
            foreach (MasterController m in a)
            {
                m.SpawnWeapons(1);
            }
            ChangeGameStateRPC(GamePhase.Starting);
            foreach (UIManager u in uiList)
            {
                u.UpdateText("Waiting for player to join");
            }
        }
        else
        {
            foreach (MasterController m in a)
            {
                m.SpawnWeapons(2);
            }
            ChangeGameStateRPC(GamePhase.Starting);
        }
    }

    //Checks game phase on render
    public override void Render()
    {
        switch (Phase)
        {
            case GamePhase.PreGame:
                break;
            case GamePhase.Starting:
                GameStarting();
                break;
            case GamePhase.Running:
                GameRunning();
                break;
            case GamePhase.Ending:
                GameEnding();
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

    //Checks for player count and starts a pre-game countdown if the minimum player amount is reached.
    public void GameStarting()
    {
        if (!gameTimer.IsRunning)
        {
            gameTimer = TickTimer.CreateFromSeconds(Runner, preGameTime);
        }
            
        int time = (int)gameTimer.RemainingTime(Runner);
        foreach (UIManager u in uiList)
        {
            u.UpdateText("Game starting in: " + time.ToString());
        }

        if (gameTimer.Expired(Runner))
        {
            foreach (UIManager u in uiList)
            {
                u.SetGameUI(true);
                u.SetGameMenu(false);
            }
            gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
            ChangeGameStateRPC(GamePhase.Running);
            soundManager.PlayStartFanfare();
        }
    }
    
    //Displays a countdown timer once the actual game has started.
    public void GameRunning()
    {
        int time = (int)gameTimer.RemainingTime(Runner);
        foreach (UIManager u in uiList)
        {
            u.UpdateTime(time.ToString());
        }

        if (gameTimer.Expired(Runner))
        {
            ChangeGameStateRPC(GamePhase.Ending);
            gameTimer = TickTimer.CreateFromSeconds(Runner, postGameTime);
        }
    }

    //Shows which player won the game and shuts down the networkRunner at the end of a timer.
    public void GameEnding()
    {
        foreach (UIManager u in uiList)
        {
            u.SetGameMenu(true);
        }
        string text = "";
        if (sm.GetP1Score() > sm.GetP2Score())
        {
            text = "Player 1 wins!";
            soundManager.PlayEndFanfare(true);
        }
        else if (sm.GetP1Score() < sm.GetP2Score())
        {
            text = "Player 2 wins!";
            soundManager.PlayEndFanfare(true);
        }
        else
        {
            text = "Its a draw!";
            soundManager.PlayEndFanfare(false);
        }

        foreach (UIManager u in uiList)
        {
            u.UpdateText(text);
        }

        if (gameTimer.Expired(Runner))
        {
            Runner.Shutdown();
            SceneManager.LoadScene(0);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void ChangeGameStateRPC(GamePhase phase)
    {
        Phase = phase;
    }
}
