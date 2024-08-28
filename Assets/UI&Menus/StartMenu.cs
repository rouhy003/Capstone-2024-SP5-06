using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _LobbyID;
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;

    private NetworkRunner _runnerInstance = null;

    public void StartShared()
    {
        StartGame(GameMode.Shared, _LobbyID.text);
    }

    private async void StartGame(GameMode mode, string roomName)
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
            Scene = SceneRef.FromIndex(1)
        };

        await _runnerInstance.StartGame(startGameArgs);

        if (_runnerInstance.IsServer)
        {
            _runnerInstance.LoadScene("1");
        }
    }

}
