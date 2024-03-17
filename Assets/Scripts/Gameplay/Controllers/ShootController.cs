using GameEngine.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Gameplay
{
    public class ShootController : SerializedMonoBehaviour
    {
        [SerializeField, TabGroup("Components")]
        protected Cannon _cannon;

        protected InputManager _inputManager;

        protected bool _canShoot;
        
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
            _canShoot = inputDataset.UiElement == null;
        }
        
        protected virtual void InputManagerPointerReleasedHandler(InputDataset inputDataset)
        {
            if (_canShoot) _cannon.Shoot();
        }
    }
}