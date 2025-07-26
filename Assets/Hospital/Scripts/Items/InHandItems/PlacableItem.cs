using Mirror;
using UnityEngine;
using System;

public class PlacableItem : InHandItem
{
    [Header("Settings")]
    [SerializeField] protected float _longPressTime = 0.2f;
    [SerializeField] protected LayerMask _placementMask;
    [SerializeField] protected float _maxPlaceDistance = 5f;

    [Header("References")]
    protected Transform _camera;
    [SerializeField] protected GameObject _itemPrefab;
    [SerializeField] protected GameObject _previewPrefab;

    protected GameObject _currentPreview;
    [SyncVar] protected float _pressTimer;
    [SyncVar] protected bool _isInPreviewMode;




    new public virtual void Update()
    {
        if (!isOwned) return;
        if (!_camera)
        {
            try
            {
                _camera = GetComponentInParent<ModulesObjectsBuffer>().Camera.transform;
            }
            catch { }

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isServer) ResetPressTimer();
            else CmdResetPressTimer();
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (isServer) HandlePressedButton();
            else CmdHandlePressedButton();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            if (isServer) HandleReleaseButton();
            else CmdHandleReleaseButton();
        }
    }

    #region HandleInput
    [Server]
    protected void ResetPressTimer() => _pressTimer = 0f;

    [Command] 
    protected void CmdResetPressTimer() => ResetPressTimer();

    [Server]
    protected void HandlePressedButton()
    {
        _pressTimer += Time.deltaTime;

        if (_pressTimer >= _longPressTime && !_isInPreviewMode)
        {
            _isInPreviewMode = true;
            TargetEnterPreviewMode(connectionToClient);
        }
        if (_isInPreviewMode)
        {
            TargetUpdatePreviewPosition(connectionToClient);
        }
    }

    [Command] 
    protected void CmdHandlePressedButton() => HandlePressedButton();

    [Server]
    protected void HandleReleaseButton()
    {
        if (_isInPreviewMode)
        {
            TargetTryPlaceItem(connectionToClient);
            _isInPreviewMode = false;
            TargetExitPreviewMode(connectionToClient);
        }
        else if (_pressTimer < _longPressTime)
        {
            TryInstantPlace();
        }
    }

    [Command] 
    protected void CmdHandleReleaseButton() => HandleReleaseButton();

    #endregion

    [TargetRpc]
    private void TargetEnterPreviewMode(NetworkConnectionToClient connectionToClient)
    {
        _currentPreview = Instantiate(_previewPrefab);
    }

    [TargetRpc]
    private void TargetUpdatePreviewPosition(NetworkConnectionToClient connectionToClient)
    {
        print(_currentPreview);
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _maxPlaceDistance, _placementMask))
        {
            _currentPreview.transform.position = hit.point;
            _currentPreview.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
    }

    [TargetRpc]
    private void TargetExitPreviewMode(NetworkConnectionToClient connectionToClient)
    {
        if (_currentPreview != null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
        }
    }

    [TargetRpc]
    private void TargetTryPlaceItem(NetworkConnectionToClient connectionToClient)
    {
        if (_currentPreview != null)
        {
            CmdPlaceItem(_currentPreview.transform.position, _currentPreview.transform.rotation);
        }
    }



    [Command]
    private void CmdPlaceItem(Vector3 position, Quaternion rotation)
    {
        PlaceItem(position, rotation);
    }

    [Server]
    private void TryInstantPlace()
    {
        if (Physics.Raycast(_camera.position, _camera.forward, out RaycastHit hit, _maxPlaceDistance, _placementMask))
        {
            PlaceItem(hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        }
    }

    [Server]
    private void PlaceItem(Vector3 position, Quaternion rotation)
    {
        GameObject placedItem = Instantiate(_itemPrefab, position, rotation);
        NetworkServer.Spawn(placedItem);
        onItemUsed?.Invoke(connectionToClient);
    }

    private void OnDisable()
    {
        if(_currentPreview != null) Destroy(_currentPreview);
    }
}
