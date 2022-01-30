using UnityEngine;


public class PlatformTilingConfigurationSaving : MonoBehaviour
{
    [SerializeField] private PlatformTilingConfiguration _platformTilingConfiguration;

    private void Awake()
    {
        PlatformTilingConfigurationSaver.Save(_platformTilingConfiguration);
    }
}