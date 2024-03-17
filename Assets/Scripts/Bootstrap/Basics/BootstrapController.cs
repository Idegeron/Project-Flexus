using GameEngine.Project;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Bootstrap
{
    public class BootstrapController : SerializedMonoBehaviour
    {
        [SerializeField] 
        protected ProjectScope _projectScope;

        protected void Start()
        {
            _projectScope.Build();
        }
    }
}