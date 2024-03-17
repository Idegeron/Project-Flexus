using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.UI
{
    public class AbstractScreen : UiElement
    {
        [SerializeField, TabGroup("Components")]
        protected Animator _animator;

        [SerializeField, TabGroup("Parameters")]
        protected string _displayAnimationKey;

        [SerializeField, TabGroup("Parameters")]
        protected string _hideAnimationKey;
        
        public event Action OnDisplay;
        
        public event Action OnHide;

        public void OnDisplayAnimationEnded()
        {
            OnDisplay?.Invoke();
        }

        public void OnHideAnimationEnded()
        {
            OnHide?.Invoke();
        }
        
        public virtual void Display()
        {
            _animator?.SetTrigger(_displayAnimationKey);
        }
        
        public virtual void Hide()
        {
            _animator?.SetTrigger(_hideAnimationKey);
        }
    }
}