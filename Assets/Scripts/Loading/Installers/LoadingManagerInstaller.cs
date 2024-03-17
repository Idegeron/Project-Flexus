using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameEngine.Loading
{
    public class LoadingManagerInstaller : IInstaller
    {
        [SerializeField] protected LoadingConfig LoadingConfig;
        
        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LoadingManager>(Lifetime.Singleton)
                .WithParameter("loadingConfig", LoadingConfig)
                .AsSelf();
        }
    }
   
}