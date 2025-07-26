using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapSwitcher : NetworkBehaviour
{
    [SerializeField] private Texture2D[] _darkLightmapDirection, _darkLightmapColor;
    [SerializeField] private Texture2D[] _brightLightmapDirection, _brightLightmapColor;

    private LightmapData[] _darkLightmap, _brightLightmap;

    private void Start()
    {
        List<LightmapData> darkLightmap = new List<LightmapData>();

        for(int i = 0; i<_darkLightmapDirection.Length; i++)
        {
            LightmapData data = new LightmapData();
            data.lightmapDir = _darkLightmapDirection[i];
            data.lightmapColor = _darkLightmapColor[i];
            darkLightmap.Add(data);
        }
        _darkLightmap = darkLightmap.ToArray();

        List<LightmapData> brightLightmap = new List<LightmapData>();

        for (int i = 0; i < _brightLightmapDirection.Length; i++)
        {
            LightmapData data = new LightmapData();
            data.lightmapDir = _brightLightmapDirection[i];
            data.lightmapColor = _brightLightmapColor[i];
            brightLightmap.Add(data);
        }
        _brightLightmap = brightLightmap.ToArray();
    }

    public void SetBrightLightmap()
    {
        LightmapSettings.lightmaps = _brightLightmap;
    }

    public void SetDarkLightmap()
    {
        LightmapSettings.lightmaps = _darkLightmap;
    }
}
