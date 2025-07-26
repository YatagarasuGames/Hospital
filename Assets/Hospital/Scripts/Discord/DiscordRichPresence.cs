using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using UnityEngine.AddressableAssets;

public class DiscordRichPresence : MonoBehaviour
{
    public Discord.Discord discord;
    public long clientID = 1378076336043851836; // Замените на ваш Client ID из Discord Dev Portal
    public   AssetReference smth;

    void Start()
    {
        try
        {
            discord = new Discord.Discord(clientID, (System.UInt64)Discord.CreateFlags.Default);
            var activityManager = discord.GetActivityManager();

            var activity = new Discord.Activity
            {
                State = "В меню",
                Assets =
            {
                LargeImage = "5d455463-48f7-4341-be6d-fc6f995b7af6", // Название изображения, загруженного в Discord Dev Portal
                LargeText = "Hospital",
                //SmallImage = "icon_small", // Опционально
                //SmallText = "Доп. информация"
            },
                Timestamps =
            {
                Start = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }
            };

            activityManager.UpdateActivity(activity, (result) =>
            {
                if (result == Discord.Result.Ok)
                    Debug.Log("Discord Rich Presence запущен!");
                else
                    Debug.LogError("Ошибка Discord Rich Presence: " + result);
            });
        }
        catch (System.Exception e)
        {
            Debug.Log("Discord не запущен: " + e.Message);
        }

    }

    void Update()
    {
        // Обновляем Discord (вызывать регулярно)
        if(discord != null) discord.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        // Закрываем соединение при выходе
        if (discord != null) discord.Dispose();
    }
}
