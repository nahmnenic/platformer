using UnityEngine;

namespace PixelCrew.UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        private static readonly int showKey = Animator.StringToHash("show");
        private static readonly int hideKey = Animator.StringToHash("hide");

        private Animator _animator;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetTrigger(showKey);
        }

        public void Close()
        {
            _animator.SetTrigger(hideKey);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}