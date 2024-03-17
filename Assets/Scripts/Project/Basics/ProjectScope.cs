using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameEngine.Project
{
    [ShowOdinSerializedPropertiesInInspector]
    public class ProjectScope : LifetimeScope, ISerializationCallbackReceiver, ISupportsPrefabSerialization
    {
        [SerializeField, HideInInspector]
        protected SerializationData _serializationData;

        [OdinSerialize]
        protected List<IInstaller> _installers = new ();

        SerializationData ISupportsPrefabSerialization.SerializationData
        {
            get => _serializationData;
            set => _serializationData = value;
        }
        
        [Inject]
        protected void Construct(IObjectResolver objectResolver)
        {
            _installers.ForEach(objectResolver.Inject);
        }
        
        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
            
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            foreach (var installer in _installers)
            {
                installer.Install(builder);
            }
        }

        protected virtual void OnAfterDeserialize()
        {
        }

        protected virtual void OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            UnitySerializationUtility.DeserializeUnityObject(this, ref _serializationData);
            OnAfterDeserialize();
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            OnBeforeSerialize();
            UnitySerializationUtility.SerializeUnityObject(this, ref _serializationData);
        }
        
#if UNITY_EDITOR
        [HideInTables]
        [OnInspectorGUI]
        [PropertyOrder(-2.147484E+09f)]
        private void InternalOnInspectorGUI()
        {
            EditorOnlyModeConfigUtility.InternalOnInspectorGUI(this);
        }
#endif
    }
}