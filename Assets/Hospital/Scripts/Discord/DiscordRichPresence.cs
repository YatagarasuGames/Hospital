using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEngine.AddressableAssets;

public class DiscordRichPresence : MonoBehaviour
{
    public Discord.Discord discord;
    public long clientID = 1378076336043851836; // �������� �� ��� Client ID �� Discord Dev Portal
    public   AssetReference smth;

    void Start()
    {
        try
        {
            discord = new Discord.Discord(clientID, (System.UInt64)Discord.CreateFlags.Default);
            var activityManager = discord.GetActivityManager();

            var activity = new Discord.Activity
            {
                State = "� ����",
                Assets =
            {
                LargeImage = "5d455463-48f7-4341-be6d-fc6f995b7af6", // �������� �����������, ������������ � Discord Dev Portal
                LargeText = "Hospital",
                //SmallImage = "icon_small", // �����������
                //SmallText = "���. ����������"
            },
                Timestamps =
            {
                Start = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }
            };

            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Discord.Result.Ok)
                    Debug.Log("Discord Rich Presence �������!");
                else
                    Debug.LogError("������ Discord Rich Presence: " + result);
            });
        }
        catch (System.Exception e)
        {
            Debug.Log("Discord �� �������: " + e.Message);
        }

    }

    void Update()
    {
        // ��������� Discord (�������� ���������)
        if(discord != null) discord.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        // ��������� ���������� ��� ������
        if (discord != null) discord.Dispose();
    }
}
