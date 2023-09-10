using UnityEngine;

namespace MathUtils
{
    public class Vector3Spring : ISpring<Vector3>
    {
        private readonly FloatSpring _xSpring;
        private readonly FloatSpring _ySpring;
        private readonly FloatSpring _zSpring;

        public Vector3Spring(float halflife = 1f, float dampingRatio = 0.5f)
        {
            _xSpring = new FloatSpring(halflife, dampingRatio);
            _ySpring = new FloatSpring(halflife, dampingRatio);
            _zSpring = new FloatSpring(halflife, dampingRatio);
        }

        public Vector3Spring(
            float xHalflife = 1f, float xDamping = 0.5f,
            float yHalflife = 1f, float yDamping = 0.5f,
            float zHalflife = 1f, float zDamping = 0.5f
        )
        {
            _xSpring = new FloatSpring(xHalflife, xDamping);
            _ySpring = new FloatSpring(yHalflife, yDamping);
            _zSpring = new FloatSpring(zHalflife, zDamping);
        }

        public Vector3 Value => _value;
        private Vector3 _value;

        public Vector3 GoalValue => _goalValue;
        private Vector3 _goalValue;

        public Vector3 GoalVelocity => _goalVelocity;
        private Vector3 _goalVelocity = Vector3.zero;

        public Vector3 Velocity => _velocity;
        private Vector3 _velocity;

        public float DistanceToGoal => (_value - _goalValue).magnitude;

        public Vector3 Update(float deltaTime)
        {
            _value.x = _xSpring.Update(deltaTime);
            _value.y = _ySpring.Update(deltaTime);
            _value.z = _zSpring.Update(deltaTime);

            _velocity.x = _xSpring.Velocity;
            _velocity.y = _ySpring.Velocity;
            _velocity.z = _zSpring.Velocity;

            _goalVelocity.x = _xSpring.GoalVelocity;
            _goalVelocity.y = _ySpring.GoalVelocity;
            _goalVelocity.z = _zSpring.GoalVelocity;

            return _value;
        }

        public void SetGoal(Vector3 target)
        {
            _xSpring.SetGoal(target.x);
            _ySpring.SetGoal(target.y);
            _zSpring.SetGoal(target.z);

            _goalValue.x = target.x;
            _goalValue.y = target.y;
            _goalValue.z = target.z;
        }

        public void SetValue(Vector3 value)
        {
            _xSpring.SetValue(value.x);
            _ySpring.SetValue(value.y);
            _zSpring.SetValue(value.z);
        }

        public void SetHalfLife(float halfLife)
        {
            _xSpring.SetHalfLife(halfLife);
            _ySpring.SetHalfLife(halfLife);
            _zSpring.SetHalfLife(halfLife);
        }

        public void SetDamping(float damping)
        {
            _xSpring.SetDamping(damping);
            _ySpring.SetDamping(damping);
            _zSpring.SetDamping(damping);
        }

        public Vector3 Predict(float futureDeltaTime)
        {
            return new Vector3(
                _xSpring.Predict(futureDeltaTime),
                _ySpring.Predict(futureDeltaTime),
                _zSpring.Predict(futureDeltaTime)
            );
        }
    }
}