using System;
using System.Collections.Generic;
using System.Linq;
using GameEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;

namespace GameEngine.Input
{
    public class InputManager : IInitializable
    {
        protected InputProvider _inputProvider = new();
        
        protected Dictionary<int, Touch> _touches = new();

        protected Dictionary<int, bool> _isPointerDown = new();

        protected Dictionary<int, UiElement> _previousUiElements = new();
        protected Dictionary<int, UiElement> _currentUiElements = new();
        protected Dictionary<int, UiElement> _selectedUiElements = new();

        
        public event Action<InputDataset> PointerMoved;
        public event Action<InputDataset> PointerPressed;
        public event Action<InputDataset> PointerDragged;
        public event Action<InputDataset> PointerReleased;
        
        public void Initialize()
        {
            _inputProvider.Basic.Touch1.performed += context => UpdateTouch(context.ReadValue<Pointer>(), 0);
            _inputProvider.Basic.Touch2.performed += context => UpdateTouch(context.ReadValue<Pointer>(), 1);
            _inputProvider.Basic.Touch3.performed += context => UpdateTouch(context.ReadValue<Pointer>(), 2);

            _inputProvider.Basic.Touch1.Enable();
            _inputProvider.Basic.Touch2.Enable();
            _inputProvider.Basic.Touch3.Enable();

            for (int i = 0; i < 3; i++)
            {
                _touches.Add(i, new Touch());
                
                _isPointerDown.Add(i, false);
            }
        }

        protected virtual void UpdateTouch(Pointer pointer, int inputId)
        {
            _touches[inputId].Update(pointer);

            PointerMove(pointer, inputId);
            
            if(!_isPointerDown[inputId] && pointer.Contact) PointerDown(pointer, inputId);
            else if(_isPointerDown[inputId] && pointer.Contact) PointerDrag(pointer, inputId);
            else if(_isPointerDown[inputId] && !pointer.Contact) PointerUp(pointer, inputId);
        }

        protected virtual void PointerMove(Pointer pointer, int inputId)
        {
            Vector3 position = pointer.Position;
            
            UiElement uiElement = null;
            
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = position
            };
            
            List<RaycastResult> eventRaycastResult = new List<RaycastResult>();
            
            EventSystem.current?.RaycastAll(eventData, eventRaycastResult);

            if (eventRaycastResult.Count > 0)
            {
                var firstRaycastResult = eventRaycastResult.First();
                
                if (!firstRaycastResult.gameObject.TryGetComponent(out uiElement))
                {
                    uiElement = firstRaycastResult.gameObject.GetComponentInParent<UiElement>();
                }
            }

            if (!_currentUiElements.ContainsKey(inputId))
            {
                _previousUiElements.Add(inputId, uiElement);
                _currentUiElements.Add(inputId, uiElement);
            }
            else
            {
                if (_currentUiElements[inputId] != uiElement)
                {
                    _previousUiElements[inputId] = _currentUiElements[inputId];
                    _currentUiElements[inputId] = uiElement;
                }
            }

            PointerMoved?.Invoke(new InputDataset(pointer, _touches[inputId], _currentUiElements[inputId]));
        }
        
        protected virtual void PointerDown(Pointer pointer, int inputId)
        {
            if (!_isPointerDown[inputId])
            {
                _isPointerDown[inputId] = true;

                var touch = _touches[inputId];
                
                var interactable = _currentUiElements[inputId];
                
                var inputData = new InputDataset(pointer, touch, interactable);

                if (!_selectedUiElements.ContainsKey(inputId)) _selectedUiElements.Add(inputId, interactable);
                else _selectedUiElements[inputId] = interactable;

                PointerPressed?.Invoke(inputData);
            }
        }
        
        protected virtual void PointerDrag(Pointer pointer, int inputId)
        {
            if (_isPointerDown[inputId])
            {
                var touch = _touches[inputId];
                
                var interactable = _selectedUiElements[inputId];

                var inputData = new InputDataset(pointer, touch, interactable);
                
                PointerDragged?.Invoke(inputData);
            }
        }
        
        protected virtual void PointerUp(Pointer pointer, int inputId)
        {
            if (_isPointerDown[inputId])
            {
                _isPointerDown[inputId] = false;

                var touch = _touches[inputId];
                
                var interactable = _selectedUiElements[inputId];

                var inputData = new InputDataset(pointer, touch, interactable);
                
                PointerReleased?.Invoke(inputData);
            }
        }

        public virtual Touch GetTouch(int inputId)
        {
            _touches.TryGetValue(inputId, out var touch);

            return touch;
        }

        public virtual UiElement GetPreviousUiElement(int inputId)
        {
            _previousUiElements.TryGetValue(inputId, out var interactable);

            return interactable;
        }
        
        public virtual UiElement GetCurrentUiElement(int inputId)
        {
            _currentUiElements.TryGetValue(inputId, out var interactable);

            return interactable;
        }
        
        public virtual UiElement GetSelectedUiElement(int inputId)
        {
            _selectedUiElements.TryGetValue(inputId, out var interactable);

            return interactable;
        }
    }
}