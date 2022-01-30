using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI : MonoBehaviour
{
    [SerializeField] private WinPanel _winPanel;
    [SerializeField] private LosePanel _losePanel;
    [SerializeField] private StartPanel _startPanel;

    public event UnityAction StartPanelButtonClicked;
    public event UnityAction WinPanelButtonClicked;
    public event UnityAction LosePanelButtonClicked;

    private void OnEnable()
    {
        _startPanel.ShowPanel();

        _startPanel.ContinueButtonClicked += OnStartPanelButtonClicked;
        _winPanel.ContinueButtonClicked += OnWinPanelButtonClicked;
        _losePanel.ContinueButtonClicked += OnLosePanelButtonClicked;
    }

    private void OnDisable()
    {
        _startPanel.ContinueButtonClicked -= OnStartPanelButtonClicked;
        _winPanel.ContinueButtonClicked -= OnWinPanelButtonClicked;
        _losePanel.ContinueButtonClicked -= OnLosePanelButtonClicked;
    }

    private void OnStartPanelButtonClicked()
    {
        StartPanelButtonClicked?.Invoke();
        _startPanel.ClosePanel();
    }

    private void OnWinPanelButtonClicked()
    {
        _winPanel.ClosePanel();
        WinPanelButtonClicked?.Invoke();
    }

    private void OnLosePanelButtonClicked()
    {
        _losePanel.ClosePanel();
        LosePanelButtonClicked?.Invoke();
    }


    public void ShowWinPanel()
    {
        _winPanel.ShowPanel();
    }

    public void ShowLosePanel()
    {
        _losePanel.ShowPanel();
    }
}
