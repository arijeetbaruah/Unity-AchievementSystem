using Game.Events;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;

namespace Game.Service
{
    public class CharacterSpawnerService : IService
    {
        public IEnumerator InstantiateCharacter(string characterID, Transform parant, Action<CharacterDetails> callback)
        {
            var handler = CharacterDatabase.Instance.references[characterID].InstantiateAsync(parant);
            yield return handler;
            callback?.Invoke(handler.Result.GetComponent<CharacterDetails>());
        }

        public bool Release(GameObject gameObject)
        {
            return Addressables.ReleaseInstance(gameObject);
        }
    }

}
