using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Cache
{
    public abstract class CacheProvider<T> : SerializedMonoBehaviour
    {
        [SerializeField] protected string _id;

        [SerializeField] protected Object _object;
        
        [Inject]
        protected void Construct(CacheManager cacheManager)
        {
            cacheManager.SetCache<T>(_id, _object);
        }
    }
}