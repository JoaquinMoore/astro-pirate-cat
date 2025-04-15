using UnityEngine;

public class SteeringMovement
{
    Vector2 _velocity;
    Vector2 _steering;

    readonly SteeringMovementDataSO _data;
    readonly Transform _host;

    public SteeringMovement(SteeringMovementDataSO data, Transform host)
    {
        _data = data;
        _host = host;
    }

    public SteeringMovement Seek(Vector2 target)
    {
        var distance = target - (Vector2)_host.position;
        var desiredDirection = _data.MaxSpeed * Mathf.Clamp01(distance.magnitude / _data.SlowingRadius) * distance.normalized;
        _steering += desiredDirection - _velocity;
        return this;
    }

    public Vector2 GetNextPosition()
    {
        _steering *= Time.deltaTime * _data.SteeringForce;
        return _velocity = Vector2.ClampMagnitude(_velocity + _steering, _data.MaxSpeed);
    }
}
