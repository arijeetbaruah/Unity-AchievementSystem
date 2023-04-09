using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Game.Events;
using Game.Logger;
using Game.Service;
using UnityEngine.AddressableAssets;

public class ServiceBinder : MonoBehaviour
{
    public AssetReferenceGameObject assetRefernce;
    public AssetReferenceScene nextScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Log.Initialization(new UnityLogger());
        Log.Print("hi", FilterLog.GameEvent);

        ServiceRegistry.Initialize();
        
        Bind();
    }

    private IEnumerator Start()
    {
        AuthService authService = ServiceRegistry.Get<AuthService>();

        var loadingScreen = assetRefernce.InstantiateAsync();

        yield return loadingScreen;
        yield return authService.Initialize();

        GameObject obj = loadingScreen.Result;
        DontDestroyOnLoad (obj);

        yield return Addressables.InitializeAsync();

        yield return authService.AsyncLogin();

        yield return new WaitForSeconds(3);

        yield return Addressables.LoadSceneAsync(nextScene);

        yield return new WaitForSeconds(3);

        Log.Print("SceneLoaded", FilterLog.Game);
        Destroy(obj);
        Destroy(gameObject);
    }

    private void Bind()
    {
        ServiceRegistry.Bind(new AuthService());
        ServiceRegistry.Bind(new EventManager());
        ServiceRegistry.Bind(new LoadingService());
        ServiceRegistry.Bind(new AchievementSystem());
        ServiceRegistry.Bind(new CharacterSpawnerService());
    }

    private void UnBind()
    {
        ServiceRegistry.UnBind<AuthService>();
        ServiceRegistry.UnBind<EventManager>();
        ServiceRegistry.UnBind<LoadingService>();
        ServiceRegistry.UnBind<AchievementSystem>();
        ServiceRegistry.UnBind<CharacterSpawnerService>();
    }
}
