using UnityEngine;

namespace MathUtils
{
    public class Vector2Spring : ISpring<Vector2>
    {
        private readonly FloatSpring _xSpring;
        private readonly FloatSpring _ySpring;

        public Vector2Spring(float halflife = 1f, float dampingRatio = 0.5f)
        {
            _xSpring = new FloatSpring(halflife, dampingRatio);
            _ySpring = new FloatSpring(halflife, dampingRatio);
        }

        public Vector2Spring(float xHalflife = 1f, float xDamping = 0.5f, float yHalflife = 1f, float yDamping = 0.5f)
        {
            _xSpring = new FloatSpring(xHalflife, xDamping);
            _ySpring = new FloatSpring(yHalflife, yDamping);
        }

        public Vector2 Value => _value;
        private Vector2 _value;

        public Vector2 GoalValue => _goalValue;
        private Vector2 _goalValue;

        public Vector2 GoalVelocity => _goalVelocity;
        private Vector2 _goalVelocity = Vector2.zero;

        public Vector2 Velocity => _velocity;
        public float DistanceToGoal => (_value - _goalValue).magnitude;
        private Vector2 _velocity;

        public Vector2 Update(float deltaTime)
        {
            _value.x = _xSpring.Update(deltaTime);
            _value.y = _ySpring.Update(deltaTime);

            _velocity.x = _xSpring.Velocity;
            _velocity.y = _ySpring.Velocity;

            _goalVelocity.x = _xSpring.GoalVelocity;
            _goalVelocity.y = _ySpring.GoalVelocity;

            return _value;
        }

        public void SetGoal(Vector2 target)
        {
            _xSpring.SetGoal(target.x);
            _ySpring.SetGoal(target.y);

            _goalValue.x = target.x;
            _goalValue.y = target.y;
        }

        public void SetValue(Vector2 value)
        {
            _xSpring.SetValue(value.x);
            _ySpring.SetValue(value.y);
        }

        public void SetHalfLife(float halfLife)
        {
            _xSpring.SetHalfLife(halfLife);
            _ySpring.SetHalfLife(halfLife);
        }

        public void SetDamping(float damping)
        {
            _xSpring.SetDamping(damping);
            _ySpring.SetDamping(damping);
        }

        public Vector2 Predict(float futureDeltaTime)
        {
            return new Vector2(_xSpring.Predict(futureDeltaTime), _ySpring.Predict(futureDeltaTime));
        }
    }
}