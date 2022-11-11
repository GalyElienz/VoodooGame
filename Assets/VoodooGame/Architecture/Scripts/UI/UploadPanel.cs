using System.IO;
using Extensions;
using FireBase;
using Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UploadPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject uploadPanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject gameBlock;

        [Header("Buttons and elements")]
        [SerializeField] private Button closedButton;
        [SerializeField] private Button uploadButton;
        [SerializeField] private Button uploadImageButton;
        [SerializeField] private Button confirmButton;
        
        [SerializeField] private TMP_InputField enemyNameField;
        [SerializeField] private RawImage rawEnemyImage;
        [SerializeField] private GameObject viewTextGameObject;
        
        private Texture2D texture;
        private byte[] bytes;
        private FireBaseStorage fireBaseStorage;

        private void Start()
        {
            fireBaseStorage = new FireBaseStorage();
            InitializeButtons();
        }

        private void OnEnable()
        {
            SetUserData();
        }

        private void InitializeButtons()
        {
            closedButton.AddListener(ClosedButton);
            uploadImageButton.AddListener(UploadButton);
            uploadButton.AddListener(UploadButton);
            confirmButton.AddListener(ConfirmButton);
        }

        private void ClosedButton()
        {
            uploadPanel.Deactivate();
            
            gameBlock.Activate();
            settingPanel.Activate();
        }

        private void UploadButton()
        {
            UploadImageFromGallery();
        }

        private void ConfirmButton()
        {
            Game.EnemyName = enemyNameField.text;
            Game.EnemyImage = rawEnemyImage.texture;
            
            uploadPanel.Deactivate();
            
            gameBlock.Activate();
            settingPanel.Activate();
            fireBaseStorage.UploadUserDataFromStorage(enemyNameField.text, bytes);
        }
        
        private void UploadImageFromGallery()
        {
            NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    texture = NativeGallery.LoadImageAtPath(path, 2048);
                    
                    Debug.Log("Good Upload");
                    
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    UploadPanelSetImage();
                    
                    bytes = File.ReadAllBytes(path);
                }
            });
        }
        
        private void UploadPanelSetImage()
        {
            rawEnemyImage.texture = texture;
            viewTextGameObject.Deactivate();    
            rawEnemyImage.gameObject.Activate();
        }
        
        private void SetUserData()
        {
            if (Game.EnemyImage != null)
            {
                rawEnemyImage.gameObject.Activate();
                viewTextGameObject.Deactivate();
                rawEnemyImage.texture = Game.EnemyImage;
            }
            enemyNameField.text = Game.EnemyName;
        }
    }
}