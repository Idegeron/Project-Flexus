using System;
using Cysharp.Threading.Tasks;
using GameEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace GameEngine.Loading
{
    public class LoadingManager : IInitializable
    {
        protected LoadingConfig _loadingConfig;
        
        protected UiManager _uiManager;

        protected LoadingDataset LoadingDataset;

        protected LoadingScreen _loadingScreen;
        
        [Inject]
        protected void Construct(LoadingConfig loadingConfig, UiManager uiManager)
        {
            _loadingConfig = loadingConfig;
            
            _uiManager = uiManager;
        }
        
        public async UniTask Load(LoadingDataset loadingDataset)
        {
            if (loadingDataset != null)
            {
                if (LoadingDataset != null) await Unload(LoadingDataset);
                
                LoadingDataset = loadingDataset;

                _loadingScreen = await _uiManager.ShowUiElement<LoadingScreen>(10);

                for (int i = 0; i < loadingDataset.LoadingParameters.Count; i++)
                {
                    var asyncOperationHandle =
                        loadingDataset.LoadingParameters[i].AssetReference.LoadSceneAsync(loadingDataset.LoadingParameters[i].LoadSceneMode);

                    while (asyncOperationHandle.Status != AsyncOperationStatus.Succeeded)
                    {
                        _loadingScreen.SetProgress(((float) i / loadingDataset.LoadingParameters.Count)
                                                   + (asyncOperationHandle.PercentComplete / loadingDataset.LoadingParameters.Count));

                        await UniTask.Yield();
                    }

                    if (loadingDataset.LoadingParameters[i].IsActive) SceneManager.SetActiveScene(asyncOperationHandle.Result.Scene);
                }

                await UniTask.DelayFrame(1);
                
                _uiManager.HideUiElement(_loadingScreen);
            }
        }

        public async UniTask Unload(LoadingDataset loadingDataset)
        {
            foreach (var loadingParameters in loadingDataset.LoadingParameters)
            {
                if (!loadingParameters.IsUnloadable)
                {
                    await loadingParameters.AssetReference.UnLoadScene();
                }
            }
        }

        public async void Initialize()
        {
            if (_loadingConfig.BasicsLoadingDataset != null)
            {
                foreach (var loadingParameters in _loadingConfig.BasicsLoadingDataset.LoadingParameters)
                {
                    var asyncOperationHandle =
                        loadingParameters.AssetReference.LoadSceneAsync(loadingParameters.LoadSceneMode);

                    await asyncOperationHandle;

                    if (loadingParameters.IsActive) SceneManager.SetActiveScene(asyncOperationHandle.Result.Scene);

                }
            }

            await Load(_loadingConfig.InitialLoadingDataset);
        }
    }
}