using GameEngine.Instantiate;
using GameEngine.Interactive;
using Gameplay.Others;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;

namespace GameEngine.Gameplay
{
    public class ParticleController : SerializedMonoBehaviour
    {
        [OdinSerialize, TabGroup("Components")]
        protected IInteractive _interactive;

        [SerializeField, TabGroup("Components")]
        protected Transform _particleContainer;
        
        [SerializeField, TabGroup("Components")]
        protected GameObject _particlePrefab;

        protected InstantiateManager _instantiateManager;

        [Inject]
        protected void Construct(InstantiateManager instantiateManager)
        {
            _instantiateManager = instantiateManager;
        }

        protected virtual void InteractiveInteractedHandler(IInteractiveParameters interactiveParameters)
        {
            if (interactiveParameters is CollisionInteractiveParameters collisionInteractiveParameters)
            {
                var contact = collisionInteractiveParameters.Collision.contacts[0];
                
                var particle = _instantiateManager.Instantiate<Particle>(_particlePrefab, contact.point, _particleContainer);
                
                particle.GameObject.transform.forward = -contact.normal;
                
                particle.Play();
            }
        }
        
        protected virtual void OnEnable()
        {
            _interactive.Interacted += InteractiveInteractedHandler;
        }

        protected virtual void OnDisable()
        {
            _interactive.Interacted -= InteractiveInteractedHandler;
        }
    }
}