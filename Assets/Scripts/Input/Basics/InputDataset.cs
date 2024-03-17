using System;
using GameEngine.UI;
using Sirenix.OdinInspector;

namespace GameEngine.Input
{
    [Serializable]
    public struct InputDataset
    {
        [ShowInInspector, ReadOnly] 
        public Pointer Pointer { get; }

        [ShowInInspector, ReadOnly] 
        public Touch Touch { get; }
        
        [ShowInInspector, ReadOnly] 
        public UiElement UiElement { get; }

        public InputDataset(Pointer pointer, Touch touch, UiElement uiElement)
        {
            Pointer = pointer;

            Touch = touch;

            UiElement = uiElement;
        }
        
        public override bool Equals(object obj)
        {
            return obj is InputDataset;
        }

        public override int GetHashCode()
        {
            return nameof(InputDataset).GetHashCode();
        }
    }
}