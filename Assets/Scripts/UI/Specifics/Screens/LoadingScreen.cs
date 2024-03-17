using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameEngine.UI
{
    public class LoadingScreen : AbstractScreen
    {
        [SerializeField, TabGroup("Components")]
        protected Image _loadingProgressImage;

        public void SetProgress(float value)
        {
            _loadingProgressImage.fillAmount = value;
        }
    }
}