using Game.Events;
using Game.Logger;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Service
{
    public class AuthService : IService
    {

        public IEnumerator Initialize()
        {
            yield return UnityServices.InitializeAsync();

            AuthenticationService.Instance.SignedIn += OnSignedIn;
            AuthenticationService.Instance.SignInFailed += OnSignInFailed;
            AuthenticationService.Instance.SignedOut += OnSignedOut;
            AuthenticationService.Instance.Expired += OnExpired;
        }

        public async Task AsyncLogin()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch(AuthenticationException ex)
            {
                Log.Error(ex);
            }
            catch (RequestFailedException ex)
            {
                Log.Error(ex);
            }
        }

        private void OnSignedIn()
        {
            Log.Print($"PlayerID: {AuthenticationService.Instance.PlayerId}", FilterLog.Network);
            Log.Print($"Access Token: {AuthenticationService.Instance.AccessToken}", FilterLog.Network);

            EventManager.Trigger(new OnSignedInEvent(AuthenticationService.Instance.PlayerId, AuthenticationService.Instance.AccessToken));
        }

        private void OnSignInFailed(RequestFailedException err)
        {
            Log.Error(err);
        }

        private void OnSignedOut()
        {
            Log.Print("Player signed out.", FilterLog.Network);
        }

        private void OnExpired()
        {
            Log.Print("Player session could not be refreshed and expired.", FilterLog.Network);
        }
    }
}
