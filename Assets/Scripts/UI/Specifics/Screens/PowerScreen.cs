using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace GameEngine.UI
{
    public class PowerScreen : AbstractScreen
    {
        [SerializeField, TabGroup("Components")]
        protected Scrollbar _powerScrollbar;

        [SerializeField, TabGroup("Components")]
        protected TMP_Text _valueText;
        
        public Scrollbar PowerScrollbar => _powerScrollbar;

        public TMP_Text ValueText => _valueText;
    }
}