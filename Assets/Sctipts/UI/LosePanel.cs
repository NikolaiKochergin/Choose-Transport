using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _continueButton;

    public event UnityAction ContinueButtonClicked;

    private void OnEnable()
    {
        _continueButton.onClick.AddListener(OnContinueButtonClick);
    }

    private void OnDisable()
    {
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
    }

    public void ShowPanel()
    {
        _panel.SetActive(true);
    }

    public void ClosePanel()
    {
        _panel.SetActive(false);
    }

    private void OnContinueButtonClick()
    {
        ContinueButtonClicked?.Invoke();
    }
}
