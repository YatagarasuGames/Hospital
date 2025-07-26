using Mirror;
using Steamworks;
using UnityEngine;
using TMPro;
public class ReadyUpPlayerItem : NetworkBehaviour
{
    [SerializeField] private GameObject _userIconObject;
    [SerializeField] private TMP_Text _userNick;
    [SerializeField] private ReadyUpButton _readyUpButton;

    [SyncVar(hook = nameof(OnSteamIDUpdated))]
    private ulong _userSteamId;
    [SyncVar(hook = nameof(OnNetIDUpdated))]
    private uint _userNetId;
    [SyncVar] private bool _isInited;

    public ReadyUpButton ReadyUpButton => _readyUpButton;
    public bool IsInited => _isInited;
    public ulong UserSteamId => _userSteamId;
    public uint UserNetId => _userNetId;

    private Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start()
    {
        if (!SteamManager.Initialized) return;
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }

    private void OnSteamIDUpdated(ulong oldID, ulong newID)
    {
        if (newID == 0) { print("Clearing"); _userIconObject.GetComponent<MeshRenderer>().material.mainTexture = null; return; }

        Debug.Log($"SteamID updated: {newID}");
        TryLoadAvatar();
    }

    private void OnNetIDUpdated(uint oldID, uint newID)
    {
        if (newID == 0) { _userNick.text = "Empty"; return; };

        Debug.Log($"NetID updated: {newID}");
        LoadNickname();
    }

    [Server]
    public void Init(uint userNetId, ulong newSteamId)
    {
        _userNetId = userNetId;
        _userSteamId = newSteamId;

        _isInited = true;
    }

    [Server]
    public void Reload()
    {
        RpcLoadReadyUpItemData();
    }

    [ClientRpc]
    private void RpcLoadReadyUpItemData()
    {
        TryLoadAvatar();
        LoadNickname();
    }

    private void TryLoadAvatar()
    {
        if (SteamManager.Initialized && _userSteamId != 0)
        {
            int imageHandle = SteamFriends.GetLargeFriendAvatar((CSteamID)_userSteamId);

            if (imageHandle > 0)
            {
                OnImageLoaded(new AvatarImageLoaded_t
                {
                    m_steamID = (CSteamID)_userSteamId,
                    m_iImage = imageHandle
                });
            }
        }
    }

    private void LoadNickname()
    {
        if(NetworkClient.spawned.TryGetValue(_userNetId, out NetworkIdentity player))
        {
            _userNick.text = PlayerPrefs.GetString("Nickname");
        }
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        print("OnImageLoaded");
        if (callback.m_steamID.m_SteamID == _userSteamId)
        {
            var texture = GetSteamImageAsTexture(callback.m_iImage);
            if (texture != null)
            {
                _userIconObject.GetComponent<MeshRenderer>().material.mainTexture = texture;
            }
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        if (!SteamUtils.GetImageSize(iImage, out uint width, out uint height))
            return null;

        byte[] image = new byte[width * height * 4];
        if (!SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4)))
            return null;

        Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);

        for (int y = 0; y < height; y++)
        {
            int mirrorY = (int)height - 1 - y;
            for (int x = 0; x < width; x++)
            {
                int index = (y * (int)width + x) * 4;
                int mirrorIndex = (mirrorY * (int)width + x) * 4;

                texture.SetPixel(x, y, new Color32(
                    image[mirrorIndex],
                    image[mirrorIndex + 1],
                    image[mirrorIndex + 2],
                    image[mirrorIndex + 3]
                ));
            }
        }

        texture.Apply();
        return texture;
    }

    [Server]
    public void Clear()
    {
        _userNetId = 0;
        _userSteamId = 0;
        _isInited = false;
    }
}
