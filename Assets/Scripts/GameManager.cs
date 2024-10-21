using System;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    enum GamePhase
    {
        Starting,
        Running,
        Ending
    }

    private static GameManager _singleton;
    [Networked] private GamePhase Phase { get; set; }
    [Networked] TickTimer gameTimer { get; set; }
    [Networked] int playerCount { get; set; }
    [SerializeField] private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

    public GameObject gameUI;
    public GameObject joinMenu;
    public TextMeshProUGUI timeUI;
    public TextMeshProUGUI joinText;

    public int gameTime = 100;
    public int preGameTime = 10;
    public int postGameTime = 15;

    ScoreManager sm;

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
        if (Object.HasStateAuthority)
        {
            Phase = GamePhase.Starting;
        }
        MasterController[] a = FindObjectsOfType<MasterController>();
        if (playerCount == 1)
        {
            foreach (MasterController m in a)
            {
                m.SpawnWeapons(1);
            }
        }
        else
        {
            foreach (MasterController m in a)
            {
                m.SpawnWeapons(2);
            }
        }
    }

    //Checks game phase on render
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
        if (playerCount == 1)
        {
            if (!gameTimer.IsRunning)
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, preGameTime);
            }
            
            int time = (int)gameTimer.RemainingTime(Runner);
            joinText.SetText("Game starting in: " + time.ToString());

            if (gameTimer.Expired(Runner))
            {
                gameTimer = TickTimer.CreateFromSeconds(Runner, gameTime);
                joinMenu.SetActive(false);
                gameUI.SetActive(true);
                Phase = GamePhase.Running;
            }
        }
        else
        {
            joinText.SetText("Waiting for other player to join");
        }
    }
    
    //Displays a countdown timer once the actual game has started.
    public void GameRunning()
    {
        int time = (int)gameTimer.RemainingTime(Runner);
        timeUI.SetText(time.ToString());

        if (gameTimer.Expired(Runner))
        {
            Phase = GamePhase.Ending;
            gameTimer = TickTimer.CreateFromSeconds(Runner, postGameTime);
        }
    }

    //Shows which player won the game and shuts down the networkRunner at the end of a timer.
    public void GameEnding()
    {
        joinMenu.SetActive(true);
        gameUI.SetActive(false);
        if (sm.GetP1Score() > sm.GetP2Score())
        {
            joinText.SetText("Player 1 wins!");
        }
        else if (sm.GetP1Score() < sm.GetP2Score())
        {
            joinText.SetText("Player 2 wins!");
        }
        else
        {
            joinText.SetText("Its a draw!");
        }

        if (gameTimer.Expired(Runner))
        {
            Runner.Shutdown();
            SceneManager.LoadScene(0);
        }
    }
}
