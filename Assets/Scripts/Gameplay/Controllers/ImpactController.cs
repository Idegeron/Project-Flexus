using GameEngine.Interactive;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace GameEngine.Gameplay
{
    public class ImpactController : SerializedMonoBehaviour
    {
        [OdinSerialize, TabGroup("Components")]
        protected IInteractive _interactive;

        [SerializeField, TabGroup("Components")]
        protected Renderer _renderer;
        
        [SerializeField, Title("Impact"), TabGroup("Parameters")]
        protected Shader _impactShader;

        [SerializeField, TabGroup("Parameters")]
        protected float _impactSize;

        [SerializeField, TabGroup("Parameters")]
        protected int _impactResolution;

        [SerializeField, TabGroup("Parameters")]
        protected Texture _impactTexture;

        [SerializeField, TabGroup("Parameters")]
        protected LayerMask _impactLayerMask;
        
        [SerializeField, Title("Naming"), TabGroup("Parameters")]
        protected string _baseTextureName;

        [SerializeField, TabGroup("Parameters")]
        protected string _impactTextureName;

        [SerializeField, TabGroup("Parameters")]
        protected string _impactSizeName;

        [SerializeField, TabGroup("Parameters")]
        protected string _impactPositionName;
        
        protected Material _baseMaterial;
        
        protected Material _impactMaterial;

        protected RenderTexture _bufferRenderTexture;

        protected RenderTexture _actualRenderTexture;
        
        protected virtual void InteractiveInteractedHandler(IInteractiveParameters interactiveParameters)
        {
            if (interactiveParameters is CollisionInteractiveParameters collisionInteractiveParameters)
            {
                var contact = collisionInteractiveParameters.Collision.contacts[0];

                var ray = new Ray(contact.point - contact.normal * 0.1f, contact.normal);
                
                if (Physics.Raycast(ray,  out var hit, float.PositiveInfinity, layerMask:_impactLayerMask))
                {
                    _impactMaterial.SetVector(_impactPositionName, new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                
                    Graphics.Blit(_actualRenderTexture, _bufferRenderTexture, _impactMaterial);
                
                    Graphics.Blit(_bufferRenderTexture, _actualRenderTexture);
                
                    _baseMaterial.SetTexture(_baseTextureName, _actualRenderTexture);
                }
            }
        }

        protected void Awake()
        {
            var tempMaterial = _renderer.material;
            
            var tempTexture = tempMaterial.GetTexture(_baseTextureName);

            if (tempTexture != null)
            {
                _bufferRenderTexture = new RenderTexture(tempTexture.width, tempTexture.height, 0);
                
                _actualRenderTexture = new RenderTexture(tempTexture.width, tempTexture.height, 0);
            }
            else
            {
                _bufferRenderTexture = new RenderTexture(_impactResolution, _impactResolution, 0);
                
                _actualRenderTexture = new RenderTexture(_impactResolution, _impactResolution, 0);
            }
            
            Graphics.Blit(tempTexture, _actualRenderTexture);
            
            _baseMaterial = tempMaterial;
            
            _baseMaterial.SetTexture(_baseTextureName, _actualRenderTexture);
            
            _impactMaterial = new Material(_impactShader);

            _impactMaterial.SetTexture(_impactTextureName, _impactTexture);
            
            _impactMaterial.SetFloat(_impactSizeName, _impactSize);
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