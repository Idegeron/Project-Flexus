using System;
using GameEngine.Interactive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameEngine.Gameplay
{
    public class Environment : SerializedMonoBehaviour, IInteractive
    {
        public event Action<IInteractiveParameters> Interacted;
        
        protected virtual void OnCollisionEnter(Collision collision)
        {
            Interacted?.Invoke(new CollisionInteractiveParameters{ Collision = collision});
        }
    }
}