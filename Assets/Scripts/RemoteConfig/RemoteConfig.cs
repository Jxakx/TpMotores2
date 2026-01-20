using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;

public class RemoteConfig : MonoBehaviour
{
    public struct userAttributes { }
    public struct appAttributes { }

    public int playerLife;
    public string enemyName1;
    public string enemyName2;
    public int appVersion;
    public bool serverOut;
    public float playerSpeed;    // Variable Nueva
    public string welcomeMessage; // Variable Nueva


    async Task InitializeRemoteConfigAsync()
    {
        
        await UnityServices.InitializeAsync();

        
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += OnFetchDataCompleted;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }


    void OnFetchDataCompleted(ConfigResponse response)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());

        playerLife = RemoteConfigService.Instance.appConfig.GetInt("PlayerLife");
        enemyName1 = RemoteConfigService.Instance.appConfig.GetString("EnemyName1");
        enemyName2 = RemoteConfigService.Instance.appConfig.GetString("EnemyName2");
        appVersion = RemoteConfigService.Instance.appConfig.GetInt("AppVersion");
        serverOut = RemoteConfigService.Instance.appConfig.GetBool("ServerOut");
        playerSpeed = RemoteConfigService.Instance.appConfig.GetFloat("PlayerSpeed", 5.0f);
        welcomeMessage = RemoteConfigService.Instance.appConfig.GetString("WelcomeMsg", "Hola");
    }
}
