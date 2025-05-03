using UnityEngine;

public class SteeringMovement
{
    private Vector2 _velocity;
    private Vector2 _steering;

    private readonly SteeringMovementDataSO _data;
    private readonly Transform _host;

    private Vector2 HostPosition => _host.position;

    public SteeringMovement(SteeringMovementDataSO data, Transform host)
    {
        _data = data;
        _host = host;
    }

    public SteeringMovement Seek(Vector2 target)
    {
        var distance = target - HostPosition;
        distance -= distance.normalized * _data.FleeRadius;

        var slowingFactor = Mathf.Clamp01((distance.magnitude - _data.FleeRadius) / _data.SlowingRadius);
        var desiredDirection = _data.MaxSpeed * slowingFactor * distance.normalized;
        _steering += desiredDirection - _velocity;

        return this;
    }

    public SteeringMovement Flee(Vector2 target)
    {
        var distance = target - HostPosition;

        if (distance.magnitude < _data.FleeRadius)
        {
            var fleeFactor = 1 - Mathf.Clamp01(distance.magnitude / _data.FleeRadius);
            var desiredDirection = _data.MaxSpeed * fleeFactor * distance.normalized;
            _steering += _velocity - desiredDirection;
        }

        return this;
    }

    public SteeringMovement Separation()
    {
        var neighbors = Physics2D.OverlapCircleAll(HostPosition, _data.SeparationRadius, LayerMask.GetMask("Boid"));
        Vector2 averagePosition = default;

        if (neighbors.Length > 0)
        {
            foreach (var boid in neighbors)
            {
                var distance = (Vector2)boid.transform.position - HostPosition;
                averagePosition += distance;
            }

            _steering -= (averagePosition / neighbors.Length).normalized * _data.MaxSpeed;
        }

        return this;
    }

    public Vector2 GetNextPosition()
    {
        _steering *= Time.deltaTime * _data.SteeringForce;
        return _velocity = Vector2.ClampMagnitude(_velocity + _steering, _data.MaxSpeed);
    }
}