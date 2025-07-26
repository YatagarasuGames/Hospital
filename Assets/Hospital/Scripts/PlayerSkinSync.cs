using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

public class PlayerSkinSync : NetworkBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _headMesh;
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [SyncVar(hook = nameof(OnSkinChanged))] public int _skinId = -1;
    private AsyncOperationHandle<Mesh> _headMeshLoader;
    private AsyncOperationHandle<Mesh> _bodyMeshLoader;

    private List<AsyncOperationHandle<Mesh>> _loadedMeshes = new List<AsyncOperationHandle<Mesh>>();
    [SyncVar] public bool _wasSkinAlreadyChanged = false;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer) return;
        _headMesh.gameObject.SetActive(false);
    }

    private async void OnSkinChanged(int oldSkinIndex, int newSkinIndex)
    {
        print("SKIN CHANGED");
        if (_wasSkinAlreadyChanged && oldSkinIndex != 0)
        {
            ReleaseModel();
            print("model released");
        }
        if (newSkinIndex == 0)
        {
            _headMesh.sharedMesh = SkinChanger.Instance.DefaultSkinHead;
            _bodyMesh.sharedMesh = SkinChanger.Instance.DefaultSkinBody;
            print("Default skin setted");
            return;

        }
        print($"Changing skin to {newSkinIndex}");
        _headMeshLoader = SkinChanger.Instance.ClassesData[newSkinIndex].HeadMesh.LoadAssetAsync<Mesh>();
        _bodyMeshLoader = SkinChanger.Instance.ClassesData[newSkinIndex].BodyMesh.LoadAssetAsync<Mesh>();

        await _headMeshLoader.Task;
        await _bodyMeshLoader.Task;

        if (_headMeshLoader.Status != AsyncOperationStatus.Succeeded || _bodyMeshLoader.Status != AsyncOperationStatus.Succeeded) return;
        _loadedMeshes.Add(_headMeshLoader);
        _loadedMeshes.Add(_bodyMeshLoader);
        print("models added");

        _bodyMesh.sharedMesh = _bodyMeshLoader.Result;
        _bodyMesh.sharedMaterials = SkinChanger.Instance.ClassesData[newSkinIndex].BodyMaterials.ToArray();
        _headMesh.sharedMesh = _headMeshLoader.Result;
        _headMesh.sharedMaterials = SkinChanger.Instance.ClassesData[newSkinIndex].HeadMaterials.ToArray();
    }

    private void ReleaseModel()
    {
        _loadedMeshes.Remove(_headMeshLoader);
        _loadedMeshes.Remove(_bodyMeshLoader);
        Addressables.Release(_headMeshLoader);
        Addressables.Release(_bodyMeshLoader);
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        print(_skinId);
        //RpcReleaseModelOnDisconnect();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        ReleaseAllModels();
    }
    private void ReleaseAllModels()
    {
        foreach (var model in _loadedMeshes)
        {
            Addressables.Release(model);
        }
        _loadedMeshes.Clear();
        print("ALL models released");
    }

    void OnDestroy()
    {
        ReleaseAllModels();
    }
}
