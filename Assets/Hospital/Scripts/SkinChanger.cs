using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinChanger : NetworkBehaviour
{
    public static SkinChanger Instance;

    [SerializeField] private List<ClassData> _classesData;
    public List<ClassData> ClassesData => _classesData;
    [SerializeField] public Mesh DefaultSkinHead;
    [SerializeField] public Mesh DefaultSkinBody;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [Server]
    public void ChangeSkin(ClassData classData, NetworkIdentity player)
    {
        for (int i = 0; i < _classesData.Count; i++)
        {
            if (classData == _classesData[i]) 
            { 
                player.GetComponent<PlayerSkinSync>()._skinId = i; 
                if(i != 0) player.GetComponent<PlayerSkinSync>()._wasSkinAlreadyChanged = true; 
                return; 
            }
        }

    }

    [Server]
    public void ResetSkin(NetworkIdentity player) 
    {
        player.GetComponent<PlayerSkinSync>()._skinId = 0;
        return;
    }
}
