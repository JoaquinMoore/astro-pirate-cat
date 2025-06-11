using System;
using UnityEngine;

[Serializable]
public class GlobalVariable<T>
{
    public bool useConstant;
    public T constantValue;
    public GlobalVariableReference<T> variableValue;

    public T Value => useConstant ? constantValue : variableValue.Value;

    public static implicit operator T(GlobalVariable<T> variable) => variable.Value;
}

public class GlobalVariableReference<T> : ScriptableObject
{
    [field: SerializeField]
    public T Value { get; set; }
}