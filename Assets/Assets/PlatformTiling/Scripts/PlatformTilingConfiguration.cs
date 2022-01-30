using UnityEngine;

[CreateAssetMenu(fileName = "PlatformTilingConfiguration", menuName = "ScriptableObjects", order = 2)]
public class PlatformTilingConfiguration : ScriptableObject
{
    [SerializeField] private float _tilingByScale;
    public float TilingByScale => _tilingByScale;
}

public static class PlatformTilingConfigurationSaver
{
    private const string TilingByScaleValue = "TilingByScale";
    
    private static float _tilingByScale = 0;

    public static float TilingByScale
    {
        get
        {
            if (_tilingByScale == 0)
            {
                _tilingByScale = PlayerPrefs.GetFloat(TilingByScaleValue);
            }

            return _tilingByScale;
        }
    }

    public static void Save(PlatformTilingConfiguration value)
    {
        PlayerPrefs.SetFloat(TilingByScaleValue, value.TilingByScale);
    }
}