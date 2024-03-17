using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Cache
{
    public abstract class CacheReceiver<T> : SerializedMonoBehaviour where T : Object
    {
        [SerializeField]
        protected string _key;

        protected CacheManager CacheManager;

        protected T _object;
        
        protected bool _isConstructed;

        public T Object => _object;

        [Inject]
        protected void Construct(CacheManager cacheManager)
        {
            CacheManager = cacheManager;

            _isConstructed = true;
            
            OnConstruct();
        }

        protected virtual void ObjectChangedHandler(Object value)
        {
            _object = value as T;
        }

        protected virtual void OnConstruct()
        {
            _object = CacheManager.GetCache<T>(_key, ObjectChangedHandler) as T;
        }
        
        protected async void OnEnable()
        {
            await UniTask.WaitWhile(() => !_isConstructed || !isActiveAndEnabled);

            if (_isConstructed)
            {
                CacheManager.SubscribeToCacheChanging<T>(_key, ObjectChangedHandler);
            }
        }

        protected void OnDisable()
        {
            if (_isConstructed)
            {
                CacheManager.UnsubscribeToCacheChanging<T>(_key, ObjectChangedHandler);
            }
        }
    }
}