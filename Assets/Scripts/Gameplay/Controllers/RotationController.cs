using GameEngine.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Gameplay
{
    public class RotationController : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Components")]
        protected Transform _transform;

        [SerializeField, TabGroup("Parameters")]
        protected Vector2 _sensitivity;

        [SerializeField, TabGroup("Parameters")]
        protected Vector2 _minOffsetAngle;

        [SerializeField, TabGroup("Parameters")]
        protected Vector2 _maxOffsetAngle;
        
        [SerializeField, TabGroup("Parameters")]
        protected Vector2 _maskOffsetAngle;

        protected InputManager _inputManager;

        [Inject]
        protected void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }
        
        protected virtual void OnEnable()
        {
            _inputManager.PointerPressed += InputManagerPointerPressedHandler;
            
            _inputManager.PointerReleased += InputManagerPointerReleasedHandler;
        }

        protected virtual void OnDisable()
        {
            _inputManager.PointerPressed -= InputManagerPointerPressedHandler;
            
            _inputManager.PointerReleased -= InputManagerPointerReleasedHandler;
        }
        
        protected virtual void InputManagerPointerPressedHandler(InputDataset inputDataset)
        {
            if (inputDataset.UiElement == null)
            {
                _inputManager.PointerMoved += InputManagerPointerMovedHandler;
            }
        }
        
        protected virtual void InputManagerPointerReleasedHandler(InputDataset inputDataset)
        {
            _inputManager.PointerMoved -= InputManagerPointerMovedHandler;
        }

        protected virtual void InputManagerPointerMovedHandler(InputDataset inputDataset)
        {
            var localEulerAngles = _transform.localEulerAngles;

            var tempX = Mathf.Clamp(((localEulerAngles.x + inputDataset.Touch.Delta.y / Screen.width
                   * -_sensitivity.x) + 180f) % 360f - 180f,
                _minOffsetAngle.x, _maxOffsetAngle.x) * _maskOffsetAngle.x;
            
            var tempY = Mathf.Clamp(((localEulerAngles.y + inputDataset.Touch.Delta.x / Screen.height
                    * _sensitivity.y) + 180f) % 360f - 180f,
                _minOffsetAngle.y, _maxOffsetAngle.y) * _maskOffsetAngle.y;
            
            _transform.localEulerAngles = new Vector3(tempX, tempY, localEulerAngles.z);
        }
    }
}