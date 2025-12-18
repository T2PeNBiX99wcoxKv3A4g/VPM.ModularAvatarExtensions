using System;

namespace io.github.ykysnk.ModularAvatarExtensions
{
    [Serializable]
    public struct ParamData : IEquatable<ParamData>
    {
        public string paramName;
        public float paramFloatValue;
        public int paramIntValue;
        public bool paramBoolValue;
        public Type paramType;

        public ParamData(string paramName, object paramValue)
        {
            paramFloatValue = 0;
            paramIntValue = 0;
            paramBoolValue = false;

            switch (paramValue)
            {
                case float f:
                    paramType = Type.Float;
                    paramFloatValue = f;
                    break;
                case int i:
                    paramType = Type.Int;
                    paramIntValue = i;
                    break;
                case bool b:
                    paramType = Type.Bool;
                    paramBoolValue = b;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paramValue), paramValue,
                        $"{paramName}'s paramValue must be float, int, or bool");
            }

            this.paramName = paramName;
        }

        public int GetIntValue()
        {
            return paramType switch
            {
                Type.Float => (int)paramFloatValue,
                Type.Int => paramIntValue,
                Type.Bool => paramBoolValue ? 1 : 0,
                _ => throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null)
            };
        }

        public float GetFloatValue()
        {
            return paramType switch
            {
                Type.Float => paramFloatValue,
                Type.Int => paramIntValue,
                Type.Bool => paramBoolValue ? 1f : 0f,
                _ => throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null)
            };
        }

        public bool GetBoolValue()
        {
            return paramType switch
            {
                Type.Float => paramFloatValue > 0,
                Type.Int => paramIntValue > 0,
                Type.Bool => paramBoolValue,
                _ => throw new ArgumentOutOfRangeException(nameof(paramType), paramType, null)
            };
        }

        public enum Type
        {
            Float,
            Int,
            Bool
        }

        public bool Equals(ParamData other) => paramName == other.paramName &&
                                               paramFloatValue.Equals(other.paramFloatValue) &&
                                               paramIntValue == other.paramIntValue &&
                                               paramBoolValue == other.paramBoolValue && paramType == other.paramType;

        public override bool Equals(object? obj) => obj is ParamData other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(paramName, paramFloatValue, paramIntValue, paramBoolValue, paramType);
    }
}