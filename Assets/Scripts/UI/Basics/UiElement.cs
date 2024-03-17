using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.UI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class UiElement : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Parameters")] 
        protected bool _isAutoReleasable;
        
        protected UiContainer _uiContainer;

        public bool IsAutoReleasable => _isAutoReleasable;
        
        public event Action<UiElement> UiContainerChanged;
        
        public void SetUiContainer(UiContainer container)
        {
            _uiContainer = container; 
            
            if (_uiContainer)
            {
                transform.SetParent(_uiContainer.transform, false);
                
                if(!gameObject.activeInHierarchy) SetActive(true);
            }
            else if (!_uiContainer)
            {
                transform.SetParent(null, false);
            
                if(gameObject.activeInHierarchy) SetActive(false);
            }
            
            UiContainerChanged?.Invoke(this);
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}