using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace GameEngine.Loading
{
    [CreateAssetMenu(menuName = "Configs/Loading Config", fileName = "Loading Config")]
    public class LoadingConfig : ScriptableObject
    {
        [SerializeField] 
        protected LoadingDataset basicsLoadingDataset;
        
        [SerializeField] 
        protected LoadingDataset initialLoadingDataset;

        public LoadingDataset BasicsLoadingDataset => basicsLoadingDataset;

        public LoadingDataset InitialLoadingDataset => initialLoadingDataset;
    }
}