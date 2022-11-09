using System;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Root;
using Runtime;
using UnityEngine;

namespace FireBase
{
    public class FireBaseAuthentication
    {
        public event Action OnFirebaseSingInEvent; 
        public event Action OnFirebaseSingUpEvent; 
        
        public string Message = "";
        private FirebaseUser user;

        public string SingInButton(string email, string password)
        {
            Coroutines.StartRoutine(SingIn(email, password));
            return Message;
        }
        
        public string SingUpButton(string email, string passwordOne, string passwordTwo)
        {
            Coroutines.StartRoutine(SingUp(email, passwordOne, passwordTwo, "userName"));
            return Message;
        }
        
        private IEnumerator SingIn(string email, string password) 
        {
            var loginTask = Game.Auth.SignInWithEmailAndPasswordAsync(email, password); 
            
            yield return new WaitUntil(predicate: () => loginTask.IsCompleted);
            if (loginTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {loginTask.Exception}");
                if (loginTask.Exception.GetBaseException() is FirebaseException firebaseEx)
                {
                    var errorCode = (AuthError)firebaseEx.ErrorCode;
 
                    var message = "Login Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WrongPassword:
                            message = "Wrong Password";
                            break;
                        case AuthError.InvalidEmail:
                            message = "Invalid Email";
                            break;
                        case AuthError.UserNotFound:
                            message = "Account does not exist";
                            break;
                    }
                    Message = message;
                }
            }
            else 
            {
                user = loginTask.Result;
                Game.User = user;
                Game.IsSingIn = true;
                Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.Email);
                Message = "Logged In";
                OnFirebaseSingInEvent?.Invoke();
                yield return new WaitForSeconds(2);
            }
        }
        
        private IEnumerator SingUp(string email, string passwordOne, string passwordTwo, string userName = "user")
        {
            if (userName == "")
            {
                Message = "Enter Name";
            }
            else if (passwordOne != passwordTwo)
            {
                Message = "Password Does Not Match!";
            }
            else 
            {
                var registerTask = Game.Auth.CreateUserWithEmailAndPasswordAsync(email, passwordOne);
                
                yield return new WaitUntil(predicate: () => registerTask.IsCompleted);
                if (registerTask.Exception != null)
                {
                    Debug.LogWarning(message: $"Failed to register task with {registerTask.Exception}");
                    var firebaseEx = registerTask.Exception.GetBaseException() as FirebaseException;
                    var errorCode = (AuthError)firebaseEx!.ErrorCode;
 
                    var message = "Register Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                    }
                    Message = message;
                }
                else
                {
                    user = registerTask.Result;
                    if (user != null)
                    {
                        var profile = new UserProfile{DisplayName = userName};

                        var profileTask = user.UpdateUserProfileAsync(profile);
                        
                        yield return new WaitUntil(predicate: () => profileTask.IsCompleted);
 
                        if (profileTask.Exception != null)
                        {
                         Debug.LogWarning(message: $"Failed to register task with {profileTask.Exception}");
                         var firebaseEx = profileTask.Exception.GetBaseException() as FirebaseException;
                         var errorCode = (AuthError)firebaseEx!.ErrorCode;
                         Message = "Username Set Failed!";
                         Debug.Log(errorCode);
                        }
                        else
                        {
                            Message = "";
                            OnFirebaseSingUpEvent?.Invoke();
                        }
                    }
                }
            }
        }
    }
}