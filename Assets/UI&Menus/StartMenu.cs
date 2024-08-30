using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField _LobbyID;
    [SerializeField] private NetworkRunner _networkRunnerPrefab = null;

    private NetworkRunner _runnerInstance = null;

    public void StartSharedPC()
    {
        StartGame(GameMode.Shared, _LobbyID.text, false);
    }

    public void StartSharedVR()
    {
        StartGame(GameMode.Shared, _LobbyID.text, true);
    }

    private async void StartGame(GameMode mode, string roomName, bool isVR)
    {
        _runnerInstance = FindObjectOfType<NetworkRunner>();
        if (_runnerInstance == null)
        {
            _runnerInstance = Instantiate(_networkRunnerPrefab);
        }

        _runnerInstance.GetComponent<LoadPCorVR>().isVR = isVR;

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
