using UnityEngine;

[CreateAssetMenu(fileName = "SteeringMovementDataSO", menuName = "Scriptable Objects/SteeringMovementDataSO")]
public class SteeringMovementDataSO : ScriptableObject
{
    public float MaxSpeed;
    public float SteeringForce;
    public float SlowingRadius;
    public float FleeRadius;
    public float SeparationRadius;
}
