using System;
using Extensions;
using FireBase;
using Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class SettingPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject uploadPanel;
        [SerializeField] private GameObject gameBlock;
        
        [Header("Buttons")]
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button helpButton;
        [SerializeField] private Button uploadButton;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button loadUserDataButton;
        
        [Header("Upload Enemy Data")]
        [SerializeField] private RawImage rawEnemyImage;
        [SerializeField] private GameObject viewTextGameObject;
        [SerializeField] private TMP_Text enemyNameText;
        
        [Header("Game block")]
        [SerializeField] private GameObject[] voodooDolls;

        private FireBaseStorage fireBaseStorage;
        private int dollIndex;
        
        private void Awake()
        {
            fireBaseStorage = new FireBaseStorage();
            InitializeButtons();
            
            if (Game.EnemyImage != null)
            {
                rawEnemyImage.gameObject.Activate();
                viewTextGameObject.Deactivate();
                rawEnemyImage.texture = Game.EnemyImage;
            }
            enemyNameText.text = Game.EnemyName;
            dollIndex = Convert.ToInt32(Game.DollIndex);

            foreach (var go in voodooDolls)
            {
                go.SetActive(false);
            }

            if (voodooDolls[dollIndex])
            {
                voodooDolls[dollIndex].SetActive(true);
            }
        }
        
        private void OnEnable()
        {
            SetUserData();
        }

        private void InitializeButtons()
        {
            startButton.AddListener(StartButton);
            exitButton.AddListener(ExitButton);
            helpButton.AddListener(HelpButton);
            uploadButton.AddListener(UploadButton);
            previousButton.AddListener(PreviousButton);
            nextButton.AddListener(NextButton);
            loadUserDataButton.AddListener(SetUserData);
        }

        private void PreviousButton()
        {
            voodooDolls[dollIndex].SetActive(false);
            dollIndex--;
            if (dollIndex < 0)
            {
                dollIndex = voodooDolls.Length - 1;
            }
            voodooDolls[dollIndex].SetActive(true);

            fireBaseStorage.UploadDollIndex(dollIndex.ToString());
            Game.DollIndex = dollIndex.ToString();
        }

        private void NextButton()
        {
            voodooDolls[dollIndex].SetActive(false);
            dollIndex++;
            if (dollIndex == voodooDolls.Length)
            {
                dollIndex = 0;
            }
            voodooDolls[dollIndex].SetActive(true);
            
            fireBaseStorage.UploadDollIndex(dollIndex.ToString());
            Game.DollIndex = dollIndex.ToString();
        }
        
        private void HelpButton()
        {
            helpPanel.Activate();
            
            settingPanel.Deactivate();
            gameBlock.Deactivate();
        }
        
        private void UploadButton()
        {
            uploadPanel.Activate();
            
            settingPanel.Deactivate();
            gameBlock.Deactivate();
        }
        
        private void ExitButton()
        {
            Game.Auth.SignOut();
            Game.IsSingIn = false;
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }

        private void StartButton()
        {
            if (Game.EnemyImage != null) SceneManager.LoadScene(sceneBuildIndex: 2);
        }
        
        private void SetUserData()
        {
            if (Game.EnemyImage != null)
            {
                rawEnemyImage.gameObject.Activate();
                viewTextGameObject.Deactivate();
                rawEnemyImage.texture = Game.EnemyImage;
            }
            enemyNameText.text = Game.EnemyName;
            
            dollIndex = Convert.ToInt32(Game.DollIndex);
            
            foreach (var go in voodooDolls)
            {
                go.SetActive(false);
            }

            if (voodooDolls[dollIndex])
            {
                voodooDolls[dollIndex].SetActive(true);
            }
        }
    }
}