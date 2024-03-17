using System;
using GameEngine.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.UI
{
    [Serializable]
    public class UiReference
    {
        [SerializeField, DrawWithUnity] 
        protected ComponentReference<UiElement> _componentReference;

        public ComponentReference<UiElement> ComponentReference => _componentReference;
    }
}