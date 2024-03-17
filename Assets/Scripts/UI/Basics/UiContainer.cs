using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace GameEngine.UI
{
    public class UiContainer : MonoBehaviour
    {
        [SerializeField] 
        protected int _layerOrder;
        
        protected UiManager _uiManager;
        
        protected List<UiElement> _uiElements = new List<UiElement>();

        public int LayerOrder => _layerOrder;
        
        [Inject]
        protected void Construct(UiManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        private void UiElementContainerChanged(UiElement element)
        {
            RemoveElement(element);
        }
        
        protected virtual void OnEnable()
        {
            _uiManager.RegisterUiContainer(this);
        }

        protected virtual void OnDisable()
        {
            _uiManager.UnregisterUiContainer(this);
        }

        public T GetElement<T>(Type elementType = null) where T : UiElement
        {
            if (elementType != null)
            {
                return _uiElements.FirstOrDefault(item => item.GetType() == elementType) as T;
            }
            else
            {
                return _uiElements.FirstOrDefault(item => item.GetType() == typeof(T)) as T;
            }
        }
        
        public void AddElement(UiElement element)
        {
            if (!_uiElements.Contains(element))
            {
                element.SetUiContainer(this);

                element.UiContainerChanged += UiElementContainerChanged;

                _uiElements.Add(element);
            }
        }

        public void RemoveElement(UiElement element)
        {
            if(!_uiElements.Contains(element)) return;
            element.UiContainerChanged -= UiElementContainerChanged;
            _uiElements.Remove(element);
        }

        public bool ContainsElement<T>(Type elementType = null) where T : UiElement
        {
            if (elementType != null)
            {
                return _uiElements.Any(
                    item => item.GetType() == elementType);
            }
            else
            {
                return _uiElements.Any(
                    item => item.GetType() == typeof(T));
            }
        }

        public void HideAllElements()
        {
            var elements = _uiElements;
            
            foreach (var element in elements)
            {
                element.SetUiContainer(null);
            }
        }
        
        public void HideAllElements<T>(Type elementType = null) where T : UiElement
        {
            var elements = _uiElements;
            
            if (elementType != null)
            {
                foreach (var element in elements)
                {
                    if (element.GetType() == elementType)
                    {
                        element.SetUiContainer(null);
                    }
                }
            }
            else
            {
                foreach (var element in elements)
                {
                    if (element.GetType() == typeof(T))
                    {
                        element.SetUiContainer(null);
                    }
                }
            }
        }
    }
}