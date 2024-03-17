using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace GameEngine.Physic
{
    public class Physical : SerializedMonoBehaviour, IPhysical
    {
        [SerializeField, TabGroup("Components")]
        protected Rigidbody _rigidbody;

        [SerializeField, TabGroup("Components"), Range(0.1f, 1f)]
        protected float _bounce;
        
        [ShowInInspector, ReadOnly, TabGroup("Info")] 
        protected Vector3 _velocity;

        protected PhysicManager _physicManager;

        public Vector3 Velocity => _velocity;

        [Inject]
        protected void Construct(PhysicManager physicManager)
        {
            _physicManager = physicManager;
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            var contact = collision.contacts[0];

            _velocity = Vector3.Lerp(transform.forward, contact.normal, _bounce) * _velocity.magnitude;

            _velocity -= _velocity * 0.1f;
        }
        
        protected virtual void OnEnable()
        {
            _physicManager.Register(this);
        }

        protected virtual void OnDisable()
        {
            _physicManager.Unregister(this);
        }

        public virtual void SetVelocity(Vector3 velocity)
        {
            _velocity = velocity;

            _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        public virtual void AddVelocity(Vector3 velocity)
        {
            _velocity += velocity;
        }
    }
}