using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] public string _LobbyID;
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;

    public TextMeshProUGUI joinText;

    private NetworkRunner _runnerInstance = null;

    public void StartSharedVR()
    {
        joinText.SetText("Joining...");
        StartGame(GameMode.Shared, _LobbyID, true);
    }

    private async void StartGame(GameMode mode, string roomName, bool isVR)
    {
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);
        }


        // Let the Fusion Runner know that we will be providing user input
        _runnerInstance.ProvideInput = true;

        var startGameArgs = new StartGameArgs()
        {
            GameMode = mode,
            SessionName = roomName,
            Scene = SceneRef.FromIndex(0)
        };

        await _runnerInstance.StartGame(startGameArgs);
    }

}
