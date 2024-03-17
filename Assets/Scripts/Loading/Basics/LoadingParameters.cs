using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace GameEngine.Loading
{
    [Serializable]
    public class LoadingParameters
    {
        [SerializeField] 
        protected AssetReference _assetReference;

        [SerializeField]
        protected LoadSceneMode _loadSceneMode;

        [SerializeField] 
        protected bool _isActive;

        [SerializeField] 
        protected bool _isUnloadable;

        public AssetReference AssetReference => _assetReference;

        public LoadSceneMode LoadSceneMode => _loadSceneMode;

        public bool IsActive => _isActive;

        public bool IsUnloadable => _isUnloadable;
    }
}