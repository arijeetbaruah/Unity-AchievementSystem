using Game.Events;
using System.Collections;
using UnityEngine;
using System;

namespace Game.Service
{
    public class CharacterSpawnerService : IService
    {
        public void InstantiateCharacter(string characterID, Transform parant = null, Action<CharacterDetails> callback = null)
        {
            EventManager.Trigger<AsyncEvent>(new AsyncEvent(InstantiateCharacterRoutine(characterID, parant, callback)));
        }

        public IEnumerator InstantiateCharacterRoutine(string characterID, Transform parant, Action<CharacterDetails> callback)
        {
            var handler = CharacterDatabase.Instance.references[characterID].InstantiateAsync(parant);
            yield return handler;
            callback?.Invoke(handler.Result.GetComponent<CharacterDetails>());
        }
    }

}
