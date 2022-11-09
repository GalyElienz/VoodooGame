using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HelpPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject gameBlock;

        [Header("Buttons")]
        [SerializeField] private Button closedButton;
        [SerializeField] private Button understandButton;

        private void Start()
        {
            closedButton.AddListener(UnderstandAndClosedButton);
            understandButton.AddListener(UnderstandAndClosedButton);
        }

        private void UnderstandAndClosedButton()
        {
            helpPanel.Deactivate();
            
            gameBlock.Activate();
            settingPanel.Activate();
        }
    }
}