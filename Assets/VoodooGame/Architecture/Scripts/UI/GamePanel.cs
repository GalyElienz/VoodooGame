using System;
using Extensions;
using Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private RawImage rawEnemyImage;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private GameObject[] voodooDolls;

        [Header("Buttons")] 
        [SerializeField] private Button backButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button leftHitButton;
        [SerializeField] private Button rightHitButton;
        [SerializeField] private Button frontHitButton;
        [SerializeField] private Button backHitButton;
        
        [SerializeField] private Button leftDeathButton;
        [SerializeField] private Button rightDeathButton;
        [SerializeField] private Button frontDeathButton;
        [SerializeField] private Button backDeathButton;
        
        private readonly int hitFromLeft = Animator.StringToHash("Hit_from_left");
        private readonly int hitFromRight = Animator.StringToHash("Hit_from_right");
        private readonly int hitFromFront = Animator.StringToHash("Hit_from_front");
        private readonly int hitFromBack = Animator.StringToHash("Hit_from_back");
        private readonly int deathLeft = Animator.StringToHash("Death_left");
        private readonly int deathRight = Animator.StringToHash("Death_right");
        private readonly int deathForward = Animator.StringToHash("Death_forward");
        private readonly int deathBackward = Animator.StringToHash("Death_backward");
        
        private Animator animator;
        private int dollIndex;
        
        private void Awake()
        {
            InitializeButtons();
            rawEnemyImage.texture = Game.EnemyImage;
            enemyNameText.text = Game.EnemyName;
            dollIndex = Convert.ToInt32(Game.DollIndex);
            
            animator = voodooDolls[dollIndex].GetComponent<Animator>();
            
            foreach (var go in voodooDolls)
            {
                go.SetActive(false);
            }

            if (voodooDolls[dollIndex])
            {
                voodooDolls[dollIndex].SetActive(true);
            }
        }
        
        private void InitializeButtons()
        {
            backButton.AddListener(BackButton);
            exitButton.AddListener(ExitButton);
            
            leftHitButton.AddListener(HitLeftButton);
            rightHitButton.AddListener(HitRightButton);
            frontHitButton.AddListener(HitFrontButton);
            backHitButton.AddListener(HitBackButton);
            
            leftDeathButton.AddListener(DeathLeftButton);
            rightDeathButton.AddListener(DeathRightButton);
            frontDeathButton.AddListener(DeathForwardButton);
            backDeathButton.AddListener(DeathBackwardButton);
        }
        
        private void BackButton()
        {
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }

        private void ExitButton()
        {
            Game.CleanUserData();
            SceneManager.LoadScene(sceneBuildIndex: 0);
        }

        private void HitLeftButton() => animator.SetTrigger(hitFromLeft);

        private void HitRightButton() => animator.SetTrigger(hitFromRight);
        
        private void HitFrontButton() => animator.SetTrigger(hitFromFront);
        
        private void HitBackButton() => animator.SetTrigger(hitFromBack);
        
        private void DeathLeftButton() => animator.SetTrigger(deathLeft);
        
        private void DeathRightButton() => animator.SetTrigger(deathRight);
        
        private void DeathForwardButton() => animator.SetTrigger(deathForward);
        
        private void DeathBackwardButton() => animator.SetTrigger(deathBackward);
    }
}