using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _continueButton;

    public event UnityAction ContinueButtonClicked;

    public void ShowPanel()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _panel.SetActive(true);
    }

    public void ClosePanel()
    {
        _panel.SetActive(false);
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
    }

    private void OnContinueButtonClick()
    {
        ContinueButtonClicked?.Invoke();
    }
}
