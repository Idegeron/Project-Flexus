using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameEngine.UI
{
    [CreateAssetMenu(menuName = "Configs/UI Config", fileName = "UI Config")]
    public class UiConfig : SerializedScriptableObject
    {
        [OdinSerialize, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine, KeyLabel = "Type", ValueLabel = "Contract")]
        private Dictionary<Type, UiReference> _uiElementsReferences = new();

        public UiReference GetUiElementReference<T>(Type elementType = null) where T : UiElement
        {
            if (elementType != null)
            {
                foreach (var keyValuePair in _uiElementsReferences)
                {
                    if (keyValuePair.Key == elementType)
                    {
                        return keyValuePair.Value;
                    }
                }
            }
            else
            {
                foreach (var keyValuePair in _uiElementsReferences)
                {
                    if (keyValuePair.Key == typeof(T))
                    {
                        return keyValuePair.Value;
                    }
                }
            }
            return null;
        }
    }
}