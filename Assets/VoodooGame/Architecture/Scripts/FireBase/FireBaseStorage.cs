using System;
using System.Collections;
using Firebase.Extensions;
using Firebase.Storage;
using Root;
using Runtime;
using UnityEngine;
using UnityEngine.Networking;

namespace FireBase
{
    public class FireBaseStorage
    {
        public event Action OnLoadUserDataFromStorageEvent;
        
        public void LoadUserDataFromStorage()
        {
            Debug.Log("LoadUserDataFromStorage");
            Coroutines.StartRoutine(LoadUserEnemyName());
            Coroutines.StartRoutine(LoadDollIndex());
            LoadImage();
        }

        public void UploadDollIndex(string dollIndex)
        {
            Coroutines.StartRoutine(UpdateDollIndexDatabase(dollIndex));
        }

        public void UploadUserDataFromStorage(string enemyName, byte[] bytes)
        {
            if (bytes != null ) UploadImage(bytes);
            Coroutines.StartRoutine(UpdateEnemyName(enemyName));
        }

        private void UploadImage(byte[] bytes)
        {
            var uploadReference  = FirebaseStorage.DefaultInstance.RootReference.Child("enemyImage").Child(Game.UserId);
            
            var newMetaData = new MetadataChange { ContentType = "image/png" };
            
            uploadReference.PutBytesAsync(bytes, newMetaData).ContinueWith(task =>
            {
                if (task.IsCompleted || task.IsCanceled)
                {
                    if (task.Exception != null) Debug.Log(task.Exception.ToString());
                }
                else
                {
                    Debug.Log("File Uploaded Successfully!");
                }
            });
        }
        
        private IEnumerator UpdateEnemyName(string enemyName)
        {
            var dbTask = Game.Reference.Child("users").Child(Game.UserId).Child("enemyName").SetValueAsync(enemyName);
        
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        
            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
        }
        
        private IEnumerator UpdateDollIndexDatabase(string dollIndex)
        {
            var dbTask = Game.Reference.Child("users").Child(Game.UserId).Child("indexDoll").SetValueAsync(dollIndex);
        
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        
            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
        }

        private IEnumerator LoadImageUrl(string imageUrl)
        {
            var webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            
            yield return webRequest.SendWebRequest();
            
            var texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
            
            Game.EnemyImage = texture;
            OnLoadUserDataFromStorageEvent?.Invoke();
        }

        private void LoadImage()
        {
            var enemyImage = Game.StorageReference.Child("enemyImage").Child(Game.UserId);
            
            enemyImage.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
            {
                if (!task.IsFaulted && !task.IsCanceled)
                {
                    Coroutines.StartRoutine(LoadImageUrl(Convert.ToString(task.Result)));
                }
                else
                {
                    OnLoadUserDataFromStorageEvent?.Invoke();
                    Debug.Log(task.Exception);
                }
            });
        }

        private IEnumerator LoadUserEnemyName()
        {
            var dbTask = Game.Reference.Child("users").Child(Game.UserId).GetValueAsync();
        
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        
            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else if (dbTask.Result.Value == null)
            {
                Game.EnemyName = "";
                Coroutines.StartRoutine(UpdateEnemyName(""));
            }
            else
            {
                Game.EnemyName = dbTask.Result.Child("enemyName").Value.ToString();
            }
            Debug.Log("EnemyName:" + Game.EnemyName);
        }

        private IEnumerator LoadDollIndex()
        {
            var dbTask = Game.Reference.Child("users").Child(Game.UserId).GetValueAsync();
        
            yield return new WaitUntil(predicate: () => dbTask.IsCompleted);
        
            if (dbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {dbTask.Exception}");
            }
            else if (dbTask.Result.Value == null)
            {
                Game.DollIndex = "0";
                Coroutines.StartRoutine(UpdateDollIndexDatabase("0"));
            }
            else
            {
                Game.DollIndex = dbTask.Result.Child("indexDoll").Value.ToString();
            }
        }
    }
}