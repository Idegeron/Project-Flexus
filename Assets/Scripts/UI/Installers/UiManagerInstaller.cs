using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameEngine.UI
{
    [Serializable]
    public class UiManagerInstaller : IInstaller
    {
        [SerializeField] 
        private UiConfig _uiConfig;

        public void Install(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<UiManager>().WithParameter("uiConfig", _uiConfig).AsSelf();
        }
    }
}