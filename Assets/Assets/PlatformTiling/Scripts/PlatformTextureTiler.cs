using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PlatformTextureTiler : MonoBehaviour
{
    private const string TextureName = "_BaseMap";
    
    private static readonly int Texture = Shader.PropertyToID(TextureName);
    
    private Material _material;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        Scale();
    }

    private void Scale()
    {
        float factor = PlatformTilingConfigurationSaver.TilingByScale;
        _material.SetTextureScale(Texture, new Vector2(1, transform.lossyScale.z * factor));
    }
}