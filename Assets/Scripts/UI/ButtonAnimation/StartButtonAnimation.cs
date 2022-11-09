using Runtime;
using UnityEngine;

namespace UI.ButtonAnimation
{
    public class StartButtonAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private static readonly int Highlighted = Animator.StringToHash("Highlighted");
        private static readonly int HelpMessage = Animator.StringToHash("HelpMessage");
        
        private void OnMouseEnter()
        {
            if (Game.EnemyImage == null)
            {
                animator.SetBool(HelpMessage,true);
                animator.SetBool(Highlighted,false);
            }
        }

        private void OnMouseExit()
        {
            animator.SetBool(HelpMessage,false);
            animator.SetBool(Highlighted,false);
        }
    }
}
