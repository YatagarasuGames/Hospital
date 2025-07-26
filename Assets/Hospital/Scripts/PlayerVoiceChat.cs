using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System;

public class PlayerVoiceChat : NetworkBehaviour
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioListener _audioListener;

    [SerializeField] [Range(0f, 3f)] private float _outgoingVolume = 2f;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer) return;
        _audioListener.enabled = true;
        SteamUser.GetVoiceOptimalSampleRate();
        SteamUser.StartVoiceRecording();
    }

    private void Update()
    {
        if (!isLocalPlayer) return;
        EVoiceResult voiceResult = SteamUser.GetAvailableVoice(out uint pcbCompressed); 
        if (voiceResult == EVoiceResult.k_EVoiceResultOK)
        {
            byte[] DestBuffer = new byte[1024]; 
            uint BytesWritten;
            voiceResult = SteamUser.GetVoice(true, DestBuffer, 1024, out BytesWritten);
            print($"PcbCompressed {pcbCompressed}\nBytesWritten {BytesWritten}");
            if (voiceResult == EVoiceResult.k_EVoiceResultOK && BytesWritten > 0)
            {
                CmdSendData(DestBuffer, BytesWritten);
                
            }
        }
    }

    [Command(channel = 2)]
    private void CmdSendData(byte[] data, uint size)
    {
        RpcPlayVoice(data, size);
    }

    [ClientRpc(channel = 2, includeOwner = false)]
    private void RpcPlayVoice(byte[] data, uint size)
    {
        byte[] DestBuffer2 = new byte[22050 * 2];
        uint BytesWritten2;
        EVoiceResult ret = SteamUser.DecompressVoice(data, size, DestBuffer2, (uint)DestBuffer2.Length, out BytesWritten2, 22050);
        if (ret == EVoiceResult.k_EVoiceResultOK && BytesWritten2 > 0)
        {
            _audioSource.clip = AudioClip.Create(UnityEngine.Random.Range(100, 1000000).ToString(), 22050, 1, 22050, false);

            float[] test = new float[22050];
            for (int i = 0; i < test.Length; ++i)
            {
                test[i] = (short)(DestBuffer2[i * 2] | DestBuffer2[i * 2 + 1] << 8) / 32768.0f ;
            }
            _audioSource.clip.SetData(test, 0);
            _audioSource.Play();
        }
    }
}
