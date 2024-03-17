using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameEngine.Instantiate
{
    public class InstantiateManager : IInitializable
    {
        protected Dictionary<GameObject, List<IInstance>> _instancesPool = new();

        protected IObjectResolver _objectResolver;

        [Inject]
        protected void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }
        
        protected virtual void InstanceReleasedHandler(IInstance instance)
        {
            instance.Released -= InstanceReleasedHandler;
            
            _instancesPool[instance.SourceObject].Add(instance);
        }
        
        public T Instantiate<T>(GameObject sourceObject, Transform parent = null) where T : IInstance
        {
            return Instantiate<T>(sourceObject, Vector3.zero, Quaternion.identity, parent);
        }
        
        public T Instantiate<T>(GameObject sourceObject, Vector3 position, Transform parent = null) where T : IInstance
        {
            return Instantiate<T>(sourceObject, position, Quaternion.identity, parent);
        }
        
        public T Instantiate<T>(GameObject sourceObject, Quaternion rotation, Transform parent = null) where T : IInstance
        {
            return Instantiate<T>(sourceObject, Vector3.zero, rotation, parent);
        }

        public T Instantiate<T>(GameObject sourceObject, Vector3 position, Quaternion rotation, Transform parent = null) where T : IInstance
        {
            if (!_instancesPool.ContainsKey(sourceObject))
            {
                _instancesPool.Add(sourceObject, new List<IInstance>());
            }

            if (_instancesPool[sourceObject].Count > 0)
            {
                var instance = _instancesPool[sourceObject][0];

                var instanceTransform = instance.GameObject.transform;

                instanceTransform.SetParent(parent);
                
                instanceTransform.position = position;

                instanceTransform.rotation = rotation;

                instance.Released += InstanceReleasedHandler;
                
                _instancesPool[sourceObject].Remove(instance);

                return (T)instance;
            }
            else
            {
                var instance = _objectResolver.Instantiate(sourceObject, position, rotation, parent).GetComponent<T>();

                instance.SourceObject = sourceObject;
                
                instance.Released += InstanceReleasedHandler;

                return instance;
            }
        }
        
        public virtual void Initialize()
        {
        }
    }
}