using System;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace GameEngine.Cache
{
    public class CacheManager 
    {
        Dictionary<Type, Dictionary<string, Object>> _objectMapping = new();
        
        Dictionary<Type, Dictionary<string, List<Action<Object>>>> _actionMapping = new();
        
        public Object GetCache<T>(string id)
        {
            var type = typeof(T);
            
            if(!_objectMapping.ContainsKey(type)) _objectMapping.Add(type, new Dictionary<string, Object>());

            if (_objectMapping[type].ContainsKey(id)) return _objectMapping[type][id];
            
            return null;
        }

        public Object GetCache<T>(string id, Action<Object> callback)
        {
            SubscribeToCacheChanging<T>(id, callback);

            return GetCache<T>(id);
        }
        
        public void SetCache<T>(string id, Object value)
        {
            var type = typeof(T);
            
            if(!_objectMapping.ContainsKey(type)) _objectMapping.Add(type, new Dictionary<string,Object>());

            if (_objectMapping[type].ContainsKey(id)) _objectMapping[type][id] = value;
            else _objectMapping[type].Add(id, value);

            if (_actionMapping.ContainsKey(type) && _actionMapping[type].ContainsKey(id))
            {
                foreach (var action in _actionMapping[type][id])
                {
                    action?.Invoke(value);
                }
            }
        }

        public void SubscribeToCacheChanging<T>(string id, Action<Object> callback)
        {
            var type = typeof(T);

            if (!_actionMapping.ContainsKey(type)) _actionMapping.Add(type, new Dictionary<string, List<Action<Object>>>());

            if (!_actionMapping[type].ContainsKey(id)) _actionMapping[type].Add(id, new List<Action<Object>>());

            if (callback != null) _actionMapping[type][id].Add(callback);
        }

        public void UnsubscribeToCacheChanging<T>(string id, Action<Object> callback)
        {
            var type = typeof(T);

            if (_actionMapping.ContainsKey(type) && _actionMapping[type].ContainsKey(id))
            {
                _actionMapping[type][id].Remove(callback);
            }
        }
    }
}