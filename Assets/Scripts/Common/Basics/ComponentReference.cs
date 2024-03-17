using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameEngine.Common
{
    [Serializable]
    public class ComponentReference<TComponent> : AssetReference
    {
        public ComponentReference(string guid) : base(guid)
        {
        }

        AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> asyncOperationHandle)
        {
            var comp = asyncOperationHandle.Result.GetComponent<TComponent>();
            
            return Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);
        }
        
        public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return Addressables.ResourceManager.CreateChainOperation(base.InstantiateAsync(position, Quaternion.identity, parent), GameObjectReady);
        }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
        {
            return Addressables.ResourceManager.CreateChainOperation(
                base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
        }

        public AsyncOperationHandle<TComponent> LoadAssetAsync()
        {
            return Addressables.ResourceManager.CreateChainOperation(
                base.LoadAssetAsync<GameObject>(), GameObjectReady);
        }

        public void ReleaseInstance(AsyncOperationHandle<TComponent> asyncOperationHandle)
        {
            var component = asyncOperationHandle.Result as Component;
            
            if (component != null)
            {
                Addressables.ReleaseInstance(component.gameObject);
            }

            Addressables.Release(asyncOperationHandle);
        }
        
        public override bool ValidateAsset(Object value)
        {
            var gameObject = value as GameObject;
            
            return gameObject != null && gameObject.GetComponent<TComponent>() != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            return go != null && go.GetComponent<TComponent>() != null;
#else
            return false;
#endif
        }
    }
}