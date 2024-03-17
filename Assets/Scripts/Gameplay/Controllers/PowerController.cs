using GameEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Gameplay
{
    public class PowerController : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Components")] 
        protected Cannon _cannon;
        
        [SerializeField, TabGroup("Parameters")]
        protected float _minPower;

        [SerializeField, TabGroup("Parameters")]
        protected float _maxPower;
        
        protected UiManager _uiManager;

        protected PowerScreen _powerScreen;
        
        [Inject]
        protected void Construct(UiManager uiManager)
        {
            _uiManager = uiManager;
        }
        
        protected virtual void PowerScrollbarValueChanged(float value)
        {
            var tempPower = Mathf.Lerp(_minPower, _maxPower, value);
            
            _cannon.SetPower(tempPower);
            
            _powerScreen.ValueText.SetText(Mathf.RoundToInt(Mathf.Clamp(value * 100, 1, 100)).ToString());
        }

        protected virtual void Start()
        {
            _cannon.SetPower( Mathf.Lerp(_minPower, _maxPower, 0.5f));
        }

        protected virtual async void OnEnable()
        {
            _powerScreen = await _uiManager.ShowUiElement<PowerScreen>();
            
            if (_powerScreen != null)
            {
                _powerScreen.PowerScrollbar.ValueChanged += PowerScrollbarValueChanged;
                
                _powerScreen.Display();
            }
        }

        protected void OnDisable()
        {
            if (_powerScreen != null)
            {
                _powerScreen.PowerScrollbar.ValueChanged -= PowerScrollbarValueChanged;
                
                _uiManager.HideUiElement(_powerScreen);
            }
        }
    }
}