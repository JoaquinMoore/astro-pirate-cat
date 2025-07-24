using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Physics.Movement
{
    public class SteeringMovement
    {
        [Serializable]
        public class Data
        {
            public float MaxMovementSpeed;
            public float SteeringForce;
            public float SlowingRadius;
            public float FleeRadius;
            public float SeparationRadius;
            public float StopAcceleration;
            public LayerMask SeparationLayerMask;
        }

        private Vector2 _currentVelocity;
        private Vector2 _steering;
        private readonly Data _data;
        private readonly Transform _vehicle;
        private Vector2 _dumpVelocity;

        private Vector2 VehiclePosition => _vehicle.position;

        public SteeringMovement(Data data, Transform vehicle)
        {
            _data = data;
            _vehicle = vehicle;
        }

        public SteeringMovement PreciseArrival(Vector2 target)
        {
            Debug.Log("aaa");
            var direction = target - VehiclePosition;
            var slowingFactor = Mathf.Clamp01(direction.magnitude / _data.SlowingRadius);
            Vector2 desiredVelocity;

            if (direction.magnitude < 0.5f)
            {
                desiredVelocity = Vector2.zero;
            }
            else
            {
                desiredVelocity = (target - VehiclePosition).normalized * _data.MaxMovementSpeed * slowingFactor;
            }

            _steering += desiredVelocity - _currentVelocity;
            return this;
        }



        public SteeringMovement AddSeekForce(Vector2 target)
        {
            Debug.Log("aaa2");
            var direction = target - VehiclePosition;
            var speedFactor = Mathf.Clamp01(direction.sqrMagnitude / (_data.FleeRadius * _data.FleeRadius));
            var desiredVelocity = direction.normalized * _data.MaxMovementSpeed * speedFactor;
            _steering += desiredVelocity - _currentVelocity;

            return this;

        }

        public SteeringMovement AddArrivalForce(Vector2 target)
        {
            Debug.Log("aaa3");
            // _currentDumpVelocity = _currentVelocity;
            // var desiredDirection = Vector2.SmoothDamp(HostPosition, target, ref _currentDumpVelocity, 0.1f, _data.MaxMovementSpeed, Time.deltaTime);
            // Debug.DrawRay(HostPosition, desiredDirection, Color.yellow);
            // Debug.DrawRay(HostPosition, _currentDumpVelocity - _currentVelocity, Color.green);

            var direction = target - VehiclePosition;
            var distance = Mathf.Clamp01(direction.sqrMagnitude / (_data.FleeRadius * _data.FleeRadius));
            var desiredVelocity = direction.normalized * (distance * _data.MaxMovementSpeed);
            _steering += desiredVelocity - _currentVelocity;

            return this;
        }


        public SteeringMovement AddFleeForce(Vector2 target)
        {
            Debug.Log("aaa4");
            var distance = target - VehiclePosition;

            if (distance.magnitude < _data.FleeRadius)
            {
                var fleeFactor = 1 - Mathf.Clamp01(distance.magnitude / _data.FleeRadius);
                var desiredDirection = _data.MaxMovementSpeed * fleeFactor * distance.normalized;
                _steering += _currentVelocity - desiredDirection;
            }

            return this;
        }

        public SteeringMovement AddSeparationForce()
        {
            var neighbors =
                Physics2D.OverlapCircleAll(VehiclePosition, _data.SeparationRadius, _data.SeparationLayerMask);
            Vector2 averagePosition = default;

            if (neighbors.Length > 0)
            {
                foreach (var boid in neighbors)
                {
                    var distance = (Vector2)boid.transform.position - VehiclePosition;
                    averagePosition += distance;
                }

                _steering -= (averagePosition / neighbors.Length).normalized * _data.MaxMovementSpeed;
            }

            return this;
        }

        public SteeringMovement AddStopForce()
        {
            //Debug.Log("Aplicando freno");
            _steering -= _currentVelocity.normalized * _data.StopAcceleration;
            return this;
        }

        public Vector2 GetNextPosition()
        {
            Debug.Log("aaa6");
            // _steering *= Time.deltaTime * _data.SteeringForce;
            _currentVelocity = Vector2.ClampMagnitude(_currentVelocity + _steering * Time.deltaTime, _data.MaxMovementSpeed);
            // Debug.DrawRay(VehiclePosition, _currentVelocity, Color.red);
            _steering = Vector2.zero;
            return VehiclePosition + _currentVelocity * Time.deltaTime;
        }
    }
}