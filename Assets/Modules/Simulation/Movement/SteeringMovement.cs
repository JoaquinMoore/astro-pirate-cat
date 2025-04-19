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
        distance -= distance.normalized * _data.FleeRadius;
        var desiredDirection = _data.MaxSpeed * Mathf.Clamp01(distance.magnitude / _data.SlowingRadius) * Mathf.Clamp01(distance.magnitude / _data.FleeRadius) * distance.normalized;
        _steering += desiredDirection - _velocity;
        return this;
    }

    public SteeringMovement Flee(Vector2 target)
    {
        var distance = target - (Vector2)_host.position;

        if (distance.magnitude < _data.FleeRadius)
        {
            var desiredDirection = _data.MaxSpeed * distance.normalized;
            _steering += _velocity - desiredDirection;
        }

        return this;
    }

    public Vector2 GetNextPosition()
    {
        _steering *= Time.deltaTime * _data.SteeringForce;
        return _velocity = Vector2.ClampMagnitude(_velocity + _steering, _data.MaxSpeed);
    }
}
