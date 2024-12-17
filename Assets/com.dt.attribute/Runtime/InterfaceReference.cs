using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dt.Attribute
{
    [Serializable]
    public class InterfaceReference<TInterface, TObject>
        where TInterface : class where TObject : Object
    {
        [SerializeField, HideInInspector]
        private TObject underlyingValue;

        public TInterface Value
        {
            get => this.underlyingValue switch
            {
                null => null,
                TInterface @interface => @interface,
                _ => throw new InvalidOperationException(
                    $"{nameof(this.underlyingValue)} needs to be of type {typeof(TInterface).Name}!")
            };

            set => this.underlyingValue = value switch
            {
                null => null,
                TObject newValue => newValue,
                _ => throw new ArgumentException(
                    $"{value} needs to be of type {typeof(TObject).Name}!")
            };
        }

        public TObject UnderlyingValue
        {
            get => this.underlyingValue;
            set => this.underlyingValue = value;
        }

        public InterfaceReference()
        {
        }

        public InterfaceReference(TObject underlyingValue)
        {
            this.underlyingValue = underlyingValue;
        }

        public InterfaceReference(TInterface @interface)
        {
            this.underlyingValue = @interface as TObject;
        }
    }

    [Serializable]
    public class InterfaceReference<TInterface> : InterfaceReference<TInterface, Object>
        where TInterface : class
    {
        public InterfaceReference(TInterface @interface) : base(@interface)
        {
        }
    }
}