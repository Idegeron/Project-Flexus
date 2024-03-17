using UnityEngine;

namespace GameEngine.Cache
{
    public class CameraReceiver : CacheReceiver<Camera>
    {
        [SerializeField]
        protected Canvas _canvas;

        protected override void ObjectChangedHandler(Object value)
        {
            base.ObjectChangedHandler(value);

            _canvas.worldCamera = _object;
        }

        protected override void OnConstruct()
        {
            base.OnConstruct();

            _canvas.worldCamera = _object;
        }
    }
}