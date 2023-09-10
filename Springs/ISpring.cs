namespace MathUtils
{
    public interface ISpring<TValue>
    {
        public TValue Value { get; }
        public TValue GoalValue { get; }
        public TValue GoalVelocity { get; }
        public TValue Velocity { get; }
        public float DistanceToGoal { get; }

        public TValue Update(float deltaTime);
        public void SetGoal(TValue target);
        public void SetValue(TValue value);

        public void SetHalfLife(float halfLife);
        public void SetDamping(float damping);

        public TValue Predict(float futureDeltaTime);
    }

    // This allows us to define something has having one value type but another velocity type.
    // (Quaternions in particular return a Quaternion value but their velocity is a Vector3.)
    public interface ISpring<TValue, UVelocity>
    {
        public TValue Value { get; }
        public TValue GoalValue { get; }

        public UVelocity GoalVelocity { get; }
        public UVelocity Velocity { get; }

        public TValue Update(float deltaTime);
        public void SetGoal(TValue target);
        public void SetValue(TValue value);

        public TValue Predict(float futureDeltaTime);
    }
}