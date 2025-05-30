using System;
using UnityEngine;

namespace Physics.Movement
{
    public class SteeringMovement
    {
        private readonly Data _data;
        private readonly Transform _host;

        private Vector2 _currentVelocity;
        private Vector2 _steering;

        public SteeringMovement(Data data, Transform host)
        {
            _data = data;
            _host = host;
        }

        private Vector2 HostPosition2D => _host.position;

        public SteeringMovement AddSeekForce(Vector2 target)
        {
            var distance = target - HostPosition2D;
            distance -= distance.normalized * _data.FleeRadius;
            var slowingFactor = Mathf.Clamp01((distance.magnitude - _data.FleeRadius) / _data.SlowingRadius);
            var desiredDirection = _data.MaxSpeed * slowingFactor * distance.normalized;
            _steering += desiredDirection - _currentVelocity;

            return this;
        }

        public SteeringMovement AddFleeForce(Vector2 target)
        {
            var distance = target - HostPosition2D;

            if (distance.magnitude < _data.FleeRadius)
            {
                var fleeFactor = 1 - Mathf.Clamp01(distance.magnitude / _data.FleeRadius);
                var desiredDirection = _data.MaxSpeed * fleeFactor * distance.normalized;
                _steering += _currentVelocity - desiredDirection;
            }

            return this;
        }

        public SteeringMovement AddSeparationForce()
        {
            var neighbors =
                Physics2D.OverlapCircleAll(HostPosition2D, _data.SeparationRadius, _data.SeparationLayerMask);
            Vector2 averagePosition = default;

            if (neighbors.Length > 0)
            {
                foreach (var boid in neighbors)
                {
                    var distance = (Vector2)boid.transform.position - HostPosition2D;
                    averagePosition += distance;
                }

                _steering -= (averagePosition / neighbors.Length).normalized * _data.MaxSpeed;
            }

            return this;
        }

        public Vector2 GetNextPosition()
        {
            _steering *= Time.deltaTime * _data.SteeringForce;
            return _currentVelocity = Vector2.ClampMagnitude(_currentVelocity + _steering, _data.MaxSpeed);
        }

        [Serializable]
        public class Data
        {
            public float MaxSpeed;
            public float SteeringForce;
            public float SlowingRadius;
            public float FleeRadius;
            public float SeparationRadius;
            public LayerMask SeparationLayerMask;
        }
    }
}