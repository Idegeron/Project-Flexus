using System;
using GameEngine.Instantiate;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Gameplay.Others
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Particle : SerializedMonoBehaviour, IInstance
    {
        [SerializeField, TabGroup("Components")]
        protected ParticleSystem _particleSystem;
        
        public GameObject GameObject => gameObject;

        public GameObject SourceObject { get; set; }

        public event Action<IInstance> Released;

        protected virtual void OnParticleSystemStopped()
        {
            gameObject.SetActive(false);
            
            Released?.Invoke(this);
        }
        
        public virtual void Play(bool withChildren = true)
        {
            gameObject.SetActive(true);
            
            _particleSystem.Play(withChildren);
        }
    }
}