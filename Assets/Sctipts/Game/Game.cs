using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private Player _player;
    [SerializeField] private int _nextLevelIndex;
    [SerializeField] private int _currentLevelIndex;
    [SerializeField] private GameData _data;

    public int LevelNumber => _currentLevelIndex;
    public int NextLevelNumber => _nextLevelIndex;


    private void Awake()
    {
        Amplitude amplitude = Amplitude.Instance;
        amplitude.logging = true;
        amplitude.init("ded957f7f15088ba152d1f4bd0f97ec3");

        _data.Load();

        IDictionary<string, object> countSessions = new Dictionary<string, object>();
        countSessions.Add("count", _data.GetSessionsCount());
        Amplitude.Instance.logEvent("game_start", countSessions);
    }

    private void OnEnable()
    {
        _ui.StartPanelButtonClicked += OnStartButtonClick;
        _ui.LosePanelButtonClicked += OnRestartButtonClick;
        _ui.WinPanelButtonClicked += OnContinueButtonClick;
        _player.Finished += PlayerFinished;
        _player.Failed += PlayerFailed;
    }

    private void OnDisable()
    {
        _ui.StartPanelButtonClicked -= OnStartButtonClick;
        _ui.LosePanelButtonClicked -= OnRestartButtonClick;
        _ui.WinPanelButtonClicked -= OnContinueButtonClick;
        _player.Finished -= PlayerFinished;
        _player.Failed -= PlayerFailed;
    }


    private void OnStartButtonClick()
    {
        _player.StartMove();

        IDictionary<string, object> levelNumber = new Dictionary<string, object>();
        levelNumber.Add("level", _currentLevelIndex+1);

        Amplitude.Instance.logEvent("level_start",levelNumber);
    }

    private void OnRestartButtonClick()
    {
        SceneManager.LoadScene(_currentLevelIndex);

        IDictionary<string, object> levelNumber = new Dictionary<string, object>();
        levelNumber.Add("level", _currentLevelIndex + 1);

        Amplitude.Instance.logEvent("restart",levelNumber);
    }

    private void OnContinueButtonClick()
    {
        SceneManager.LoadScene(_nextLevelIndex);
    }

    private void PlayerFinished()
    {
        _data.Save();
        _ui.ShowWinPanel();

        IDictionary<string, object> levelNumber = new Dictionary<string, object>();
        levelNumber.Add("level", _currentLevelIndex + 1);

        Amplitude.Instance.logEvent("level_complete",levelNumber);
    }

    private void PlayerFailed()
    {
        _ui.ShowLosePanel();

        IDictionary<string, object> levelNumber = new Dictionary<string, object>();
        levelNumber.Add("level", _currentLevelIndex + 1);

        Amplitude.Instance.logEvent("fail",levelNumber);
    }
}
