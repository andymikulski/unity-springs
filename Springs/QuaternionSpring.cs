using MathUtils;
using UnityEngine;

public class QuaternionSpring : ISpring<Quaternion, Vector3>
{
    private static (Quaternion, Vector3) simple_spring_damper_exact_quat(
        Quaternion x,
        Vector3 v,
        Quaternion x_goal,
        float halflife,
        float dt)
    {
        float y = SpringMath.HalflifeToDamping(halflife) / 2.0f;

        var j0 = QuaternionToScaledAngleAxis(x * Quaternion.Inverse(x_goal));
        var j1 = (v + j0) * y;

        float eydt = SpringMath.FastNegExp(y * dt);

        x = QuaternionFromScaledAngleAxis(eydt * (j0 + j1 * dt)) * x_goal;
        v = eydt * (v - j1 * y * dt);

        return (x, v);
    }


    private static Vector3 QuaternionToScaledAngleAxis(Quaternion quat)
    {
        Vector3 scaledAngleAxis;
        float angle = 2f * Mathf.Acos(quat.w);
        float s = Mathf.Sin(angle / 2f);
        if (s != 0f)
        {
            scaledAngleAxis = new Vector3(quat.x / s, quat.y / s, quat.z / s) * angle;
        }
        else
        {
            scaledAngleAxis = Vector3.zero;
        }

        return scaledAngleAxis;
    }

    private static Quaternion QuaternionFromScaledAngleAxis(Vector3 scaledAngleAxis)
    {
        Quaternion quat;
        float angle = scaledAngleAxis.magnitude;
        Vector3 axis = scaledAngleAxis.normalized;
        quat.w = Mathf.Cos(angle / 2f);
        quat.x = axis.x * Mathf.Sin(angle / 2f);
        quat.y = axis.y * Mathf.Sin(angle / 2f);
        quat.z = axis.z * Mathf.Sin(angle / 2f);
        return quat;
    }

    // ------------------------


    public Quaternion Value { get; }
    private Quaternion currentValue;

    public Quaternion GoalValue { get; }
    private Quaternion _goalValue;

    public Vector3 GoalVelocity { get; }
    private Vector3 _goalVelocity;

    public Vector3 Velocity { get; }
    private Vector3 currentVelocity;

    private float halflife;

    public QuaternionSpring(float half_life = 1f)
    {
        this.halflife = half_life;
    }

    public Quaternion Update(float deltaTime)
    {
        var result =
            simple_spring_damper_exact_quat(currentValue, currentVelocity, _goalValue, halflife, Time.deltaTime);

        currentValue = result.Item1;
        currentVelocity = result.Item2;

        return currentValue;
    }

    public void SetGoal(Quaternion target)
    {
        _goalValue = target;
    }

    public void SetValue(Quaternion value)
    {
        SetGoal(value);
        currentValue = value;
        currentVelocity = Vector3.zero;
    }

    public Quaternion Predict(float futureDeltaTime)
    {
        var result =
            simple_spring_damper_exact_quat(currentValue, currentVelocity, _goalValue, halflife, futureDeltaTime);

        return result.Item1;
    }
}