using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GameEngine.UI
{
    public class Scrollbar : UiElement
    {
        [SerializeField, TabGroup("Components")] 
        protected UnityEngine.UI.Scrollbar _scrollbar;

        public event Action<float> ValueChanged
        {
            add => _scrollbar.onValueChanged.AddListener(new UnityAction<float>(value));
            remove => _scrollbar.onValueChanged.RemoveListener(new UnityAction<float>(value));
        }
    }
}