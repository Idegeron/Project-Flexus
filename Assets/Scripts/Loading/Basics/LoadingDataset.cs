using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Loading
{
    [CreateAssetMenu(menuName = "Datasets/Loading Dataset", fileName = "Loading Dataset")]
    public class LoadingDataset : SerializedScriptableObject
    {
        [SerializeField] 
        protected List<LoadingParameters> _loadingParameters;

        public List<LoadingParameters> LoadingParameters => _loadingParameters;
    }
}