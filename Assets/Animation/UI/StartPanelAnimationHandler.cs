using UnityEngine;

public class StartPanelAnimationHandler : MonoBehaviour
{
    // Used as trigger in start panel animation
    private void HandleOffStartPanel()
    {
        gameObject.SetActive(false);
    }
}
