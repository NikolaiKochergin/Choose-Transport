using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _continueButton;

    public Animator _startPanelAnimator;

    private void Awake()
    {
        _startPanelAnimator = _panel.GetComponent<Animator>();
        if (_startPanelAnimator == null)
            throw new ArgumentNullException(nameof(_startPanelAnimator));
    }

    public event UnityAction ContinueButtonClicked;

    public void ShowPanel()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _panel.SetActive(true);
    }

    public void ClosePanel()
    {
        _startPanelAnimator.SetTrigger(StartPanelAnimatorParams.FadeOut);
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
    }

    private void OnContinueButtonClick()
    {
        ContinueButtonClicked?.Invoke();
    }
}
