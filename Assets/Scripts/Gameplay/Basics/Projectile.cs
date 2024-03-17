using System;
using GameEngine.Instantiate;
using GameEngine.Interactive;
using GameEngine.Physic;
using Gameplay.Others;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : Physical, IInstance, IInteractive
    {
        [SerializeField, TabGroup("Components")]
        protected MeshFilter _meshFilter;

        [SerializeField, TabGroup("Components")]
        protected MeshCollider _meshCollider;

        [SerializeField, TabGroup("Components")]
        protected GameObject _releaseParticlePrefab;
        
        [SerializeField, TabGroup("Parameters")]
        protected int _maxCollisionCount;

        protected int _collisionCount;

        protected InstantiateManager _instantiateManager;
        
        public GameObject GameObject => gameObject;
        
        public GameObject SourceObject { get; set; }

        public event Action<IInteractiveParameters> Interacted;
        
        public event Action<IInstance> Released;

        [Inject]
        protected void Construct(InstantiateManager instantiateManager)
        {
            _instantiateManager = instantiateManager;
        }
        
        protected override void OnCollisionEnter(Collision collision)
        {
            base.OnCollisionEnter(collision);

            Interacted?.Invoke(new CollisionInteractiveParameters { Collision = collision});

            _collisionCount++;

            if (_collisionCount >= _maxCollisionCount)
            {
                if (_releaseParticlePrefab != null)
                {
                    _instantiateManager.Instantiate<Particle>(_releaseParticlePrefab, transform.position).Play();
                }
                
                gameObject.SetActive(false);

                Released?.Invoke(this);
            }
        }

        public void SetMesh(Mesh mesh)
        {
            _meshFilter.sharedMesh = mesh;

            _meshCollider.sharedMesh = mesh;
        }
        
        public void Shoot(float power)
        {
            gameObject.SetActive(true);
            
            _velocity = Vector3.zero;

            _collisionCount = 0;

            AddVelocity(transform.forward * power);
        }
    }
}