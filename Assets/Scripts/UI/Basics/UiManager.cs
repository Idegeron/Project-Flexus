using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace GameEngine.UI
{
    public class UiManager : IInitializable
    {
        protected UiConfig _uiConfig;

        protected List<UiContainer> _uiContainers = new();

        protected Dictionary<UiElement, AsyncOperationHandle<UiElement>> _uiElementOperationsMapping = new();

        [Inject]
        protected void Construct(UiConfig uiConfig)
        {
            _uiConfig = uiConfig;
        }

        protected UiContainer GetUiContainer(int layerOrder)
        {
            foreach (var uiContainer in _uiContainers)
            {
                if (uiContainer.LayerOrder == layerOrder) return uiContainer;
            }

            return _uiContainers.First();
        }

        public T GetUiElement<T>(Type uiElementType = null) where T : UiElement
        {
            foreach (var container in _uiContainers)
            {
                T element = container.GetElement<T>(uiElementType);
                if (element) return element;
            }

            return null;
        }

        public T GetUiElement<T>(UiContainer uiContainer, Type uiElementType = null) where T : UiElement
        {
            return uiContainer.GetElement<T>(uiElementType);
        }

        public bool ContainsUiElement<T>(Type uiElementType = null) where T : UiElement
        {
            foreach (var container in _uiContainers)
            {
                if (container.ContainsElement<T>()) return true;
            }

            return false;
        }

        public bool ContainsUiElement<T>(UiContainer uiContainer, Type uiElementType = null) where T : UiElement
        {
            return uiContainer.ContainsElement<T>(uiElementType);
        }

        public async UniTask<T> ShowUiElement<T>(Type uiElementType = null) where T : UiElement
        {
            var uiReference = _uiConfig.GetUiElementReference<T>(uiElementType);

            var asyncOperationHandler = uiReference.ComponentReference.InstantiateAsync();

            await asyncOperationHandler;

            if (asyncOperationHandler.IsDone && asyncOperationHandler.Result != null)
            {
                T uiElement = asyncOperationHandler.Result as T;

                _uiElementOperationsMapping.Add(asyncOperationHandler.Result, asyncOperationHandler);

                GetUiContainer(1)?.AddElement(uiElement);

                return uiElement;
            }

            return default;
        }

        public async UniTask<T> ShowUiElement<T>(int layerOrder, Type uiElementType = null) where T : UiElement
        {
            var uiReference = _uiConfig.GetUiElementReference<T>(uiElementType);

            var asyncOperationHandler = uiReference.ComponentReference.InstantiateAsync();

            await asyncOperationHandler;

            if (asyncOperationHandler.IsDone && asyncOperationHandler.Result != null)
            {
                T uiElement = asyncOperationHandler.Result as T;

                _uiElementOperationsMapping.Add(asyncOperationHandler.Result, asyncOperationHandler);

                GetUiContainer(layerOrder)?.AddElement(uiElement);

                return uiElement;
            }

            return default;
        }

        public async UniTask<T> ShowUiElement<T>(UiContainer uiContainer, Type uiElementType = null) where T : UiElement
        {
            if (uiContainer != null)
            {
                var uiReference = _uiConfig.GetUiElementReference<T>(uiElementType);
                
                var asyncOperationHandler = uiReference.ComponentReference.InstantiateAsync();

                await asyncOperationHandler;

                if (asyncOperationHandler.IsDone && asyncOperationHandler.Result != null)
                {
                    T uiElement = asyncOperationHandler.Result as T;

                    _uiElementOperationsMapping.Add(asyncOperationHandler.Result, asyncOperationHandler);

                    uiContainer.AddElement(uiElement);

                    return uiElement;
                }
            }

            return default;
        }

        public void HideUiElement(UiElement uiElement)
        {
            if (uiElement != null) uiElement.SetUiContainer(null);

            if (uiElement.IsAutoReleasable) ReleaseUiElement(uiElement);
        }

        public void ReleaseUiElement<T>(T uiElement) where T : UiElement
        {
            var uiReference = _uiConfig.GetUiElementReference<T>(uiElement.GetType());

            if (uiReference != null && _uiElementOperationsMapping.ContainsKey(uiElement))
            {
                uiReference.ComponentReference.ReleaseInstance(_uiElementOperationsMapping[uiElement]);
            }
        }

        public void RegisterUiContainer(UiContainer uiContainer)
        {
            if (uiContainer != null && !_uiContainers.Contains(uiContainer))
            {
                _uiContainers.Add(uiContainer);
            }
        }

        public void UnregisterUiContainer(UiContainer uiContainer)
        {
            if (uiContainer != null && _uiContainers.Contains(uiContainer))
            {
                _uiContainers.Remove(uiContainer);
            }
        }

        public void Initialize()
        {
        }
    }
}