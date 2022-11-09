using Extensions;
using FireBase;
using Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SingInAndUpPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject singInPanel;
        [SerializeField] private GameObject singUpPanel;
        [SerializeField] private GameObject loaderPanel;

        [Header("Buttons")]
        [SerializeField] private Button singInTextButton;
        [SerializeField] private Button singUpTextButton;

        [Header("SingIn Panel")]
        [SerializeField] private TMP_InputField emailSingInField;
        [SerializeField] private TMP_InputField passwordSingInField;
        [SerializeField] private TMP_Text messageSingInText;
        [SerializeField] private Button singInButton;
        
        [Header("SingUp Panel")]
        [SerializeField] private TMP_InputField emailSingUpField;
        [SerializeField] private TMP_InputField passwordSingUpField;
        [SerializeField] private TMP_InputField passwordSingUpVerifyField;
        [SerializeField] private TMP_Text messageSingUpText;
        [SerializeField] private Button singUpButton;

        private FireBaseInitialize fireBaseInitialize;
        private FireBaseAuthentication fireBaseAuthentication;
        private FireBaseStorage fireBaseStorage;
        private bool isSingUp;
        
        private void Awake()
        {
            fireBaseInitialize = new FireBaseInitialize();
            fireBaseAuthentication = new FireBaseAuthentication();
            fireBaseStorage = new FireBaseStorage();
            
            fireBaseInitialize.OnFirebaseInitializedEvent += OnFirebaseInitialized;
            fireBaseAuthentication.OnFirebaseSingInEvent += OnFirebaseSingIn;
            fireBaseAuthentication.OnFirebaseSingUpEvent += OnFirebaseSingUp;
            fireBaseStorage.OnLoadUserDataFromStorageEvent += OnLoadUserDataFromStorage;

            fireBaseInitialize.FirebaseInitialized();
            loaderPanel.Deactivate();
            InitializeButton();
        }
        
        private void OnFirebaseInitialized(bool isSingIn)
        {
            if (isSingIn)
            {
                loaderPanel.Activate();
                messageSingInText.text = "Logged In";
                fireBaseStorage.LoadUserDataFromStorage();
            }
            fireBaseInitialize.OnFirebaseInitializedEvent -= OnFirebaseInitialized;
        }

        private void OnFirebaseSingUp()
        {
            SingInChooseButton();
            emailSingInField.text = emailSingUpField.text;
            passwordSingInField.text = passwordSingInField.text;
            messageSingInText.text = ""; 
            messageSingUpText.text = "";
            fireBaseAuthentication.OnFirebaseSingUpEvent -= OnFirebaseSingUp;
        }

        private void OnFirebaseSingIn()
        {   
            loaderPanel.Activate();
            messageSingInText.text = "Logged In";
            fireBaseStorage.LoadUserDataFromStorage();
            fireBaseAuthentication.OnFirebaseSingInEvent -= OnFirebaseSingIn;
        }

        private void OnLoadUserDataFromStorage()
        {
            messageSingInText.text = "";
            Debug.Log("OnLoadUserDataFromStorage");
            if (Game.EnemyImage != null) SceneManager.LoadScene(sceneBuildIndex: 2);
            else SceneManager.LoadScene(sceneBuildIndex: 1);
            fireBaseStorage.OnLoadUserDataFromStorageEvent -= OnLoadUserDataFromStorage;
        }

        private void LoadSettingScene()
        {
            if (Game.IsSingIn)
            {
                loaderPanel.Activate();
                messageSingInText.text = "Logged In";
                fireBaseStorage.LoadUserDataFromStorage();
            }
        }

        private void InitializeButton()
        {
            singInTextButton.Tractable();
            singUpTextButton.AddListener(SingUpChooseButton);
            singInTextButton.AddListener(SingInChooseButton);
            
            singInButton.AddListener(SingInButton);
            singUpButton.AddListener(SingUpButton);
        }

        private void SingInChooseButton()
        {
            singUpTextButton.Intractable();
            singInTextButton.Tractable();

            singUpPanel.Deactivate();
            singInPanel.Activate();
        }
        
        private void SingUpChooseButton()
        {
            singInTextButton.Intractable();
            singUpTextButton.Tractable();
            
            singInPanel.Deactivate();
            singUpPanel.Activate();
        }
        
        private void SingInButton()
        {
            fireBaseAuthentication.SingInButton(emailSingInField.text, passwordSingInField.text);
            messageSingInText.text = fireBaseAuthentication.Message;
            LoadSettingScene();
        }
        
        private void SingUpButton()
        {
            fireBaseAuthentication.SingUpButton(emailSingUpField.text, passwordSingUpField.text, passwordSingUpVerifyField.text);
            messageSingUpText.text = fireBaseAuthentication.Message;
        }
    }
}