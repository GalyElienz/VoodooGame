using UnityEngine;

namespace UI.ButtonAnimation
{
    [RequireComponent(typeof(Animator))]
    public class ButtonAnimation : MonoBehaviour
    {
        private Animator animator;
        private static readonly int Highlighted = Animator.StringToHash("Highlighted");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnMouseEnter()
        {
            animator.SetBool(Highlighted,true);
        }

        private void OnMouseExit()
        {
            animator.SetBool(Highlighted,false);
        }
    }
}
