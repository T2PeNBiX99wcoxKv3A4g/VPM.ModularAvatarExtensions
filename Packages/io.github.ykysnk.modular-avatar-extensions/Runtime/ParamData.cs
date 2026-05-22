using System;
using UnityEngine;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public class ParamData : IEquatable<ParamData>
    {
        public enum Type
        {
            Float,
            Int,
            Bool
        }

        [SerializeField] private string paramName;
        [SerializeField] private float paramValue;
        [SerializeField] private Type paramType;

        public ParamData(string paramName, object paramValue)
        {
            this.paramValue = 0;

            switch (paramValue)
            {
                case float f:
                    paramType = Type.Float;
                    this.paramValue = f;
                    break;
                case int i:
                    paramType = Type.Int;
                    this.paramValue = i;
                    break;
                case bool b:
                    paramType = Type.Bool;
                    this.paramValue = b ? 1f : 0f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramValue), paramValue,
                        $"{paramName}'s paramValue must be float, int, or bool");
            }

            this.paramName = paramName;
        }

        public string ParamName => paramName;
        public float ParamValue => paramValue;
        public Type ParamType => paramType;

        public bool Equals(ParamData other) => paramName == other.paramName &&
                                               paramValue.Equals(other.paramValue) &&
                                               paramType == other.paramType;

        public int GetIntValue() => (int)paramValue;
        public float GetFloatValue() => paramValue;
        public bool GetBoolValue() => paramValue > 0;

        public override bool Equals(object? obj) => obj is ParamData other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(ParamName, ParamValue, ParamType);
    }
}