using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Storage;
using Root;
using Runtime;
using UnityEngine;

namespace FireBase
{
    public class FireBaseInitialize
    {
        
        public event Action<bool> OnFirebaseInitializedEvent; 
        
        private DependencyStatus dependencyStatus;
        private DatabaseReference reference;
        private StorageReference storageReference;
        private const string StorageName = "gs://voodoogame-9c0dc.appspot.com/";
        private FirebaseAuth auth;
        private FirebaseUser user;

        public void FirebaseInitialized()
        {
            Coroutines.StartRoutine(CheckAndFixDependenciesAsync());
        }

        private IEnumerator CheckAndFixDependenciesAsync()
        {
            var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();

            yield return new WaitUntil(() => dependencyTask.IsCompleted);
            
            dependencyStatus = dependencyTask.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
                yield return new WaitForEndOfFrame();
                Coroutines.StartRoutine(CheckForAutoSingIn());
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        }
        
        private void InitializeFirebase()
        {
            auth = FirebaseAuth.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;
            storageReference = FirebaseStorage.DefaultInstance.GetReferenceFromUrl(StorageName);
            auth.StateChanged += AuthStateChanged;
            AuthStateChanged(this, null);
            
            Game.Auth = auth;
            Game.Reference = reference;
            Game.StorageReference = storageReference;
            Debug.Log("Setting up Firebase Auth!!!");
            OnFirebaseInitializedEvent?.Invoke(Game.IsSingIn);
        }

        private void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            if (auth.CurrentUser != user)
            {
                bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
                if (!signedIn && user != null)
                {
                    Debug.Log("Signed out");
                }
                user = auth.CurrentUser;
                if (signedIn)
                {
                    Debug.Log("Signed in UserId: " + user.UserId);
                    Game.UserId = user.UserId;
                    Game.IsSingIn = true;
                }
            }
        }
        
        private IEnumerator CheckForAutoSingIn()
        {
            if (user != null)
            {
                var reloadUserTask = user.ReloadAsync();

                yield return new WaitUntil(() => reloadUserTask.IsCompleted);
                
                Game.IsSingIn = true;
                Debug.Log("For Auto SingIn: true");
            }
            else
            {
                Game.IsSingIn = false;
            }
        }
    }
}