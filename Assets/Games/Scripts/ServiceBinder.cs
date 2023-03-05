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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Log.Initialization(new UnityLogger());

        ServiceRegistry.Initialize();
        
        Bind();
    }

    private IEnumerator Start()
    {

        var loadingScreen = assetRefernce.InstantiateAsync();
        
        yield return loadingScreen;

        GameObject obj = loadingScreen.Result;
        DontDestroyOnLoad (obj);

        yield return Addressables.InitializeAsync();

        yield return new WaitForSeconds(3);

        yield return SceneManager.LoadSceneAsync(1);

        yield return new WaitForSeconds(3);

        Log.Print("SceneLoaded", FilterLog.Game);
        Destroy(obj);
        Destroy(gameObject);
    }

    private void Bind()
    {
        ServiceRegistry.Bind(new EventManager());
        ServiceRegistry.Bind(new LoadingService());
        ServiceRegistry.Bind(new AchievementSystem());
    }
}