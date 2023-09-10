using MathUtils;
using UnityEngine;

public class FloatSpring : ISpring<float>
{
    private float halflife;
    private float dampingRatio;

    public float Value { get; private set; }

    public float GoalValue { get; private set; }

    public float GoalVelocity => 0f;

    public float Velocity { get; private set; }

    public float DistanceToGoal => Value - GoalValue;

    public FloatSpring(float halflife = 1f, float dampingRatio = 0.5f)
    {
        this.halflife = halflife;
        this.dampingRatio = dampingRatio;
    }

    public void SetGoal(float target)
    {
        GoalValue = target;
    }

    public void SetValue(float value)
    {
        Value = value;
        Velocity = 0f;
    }

    public void SetDamping(float damping)
    {
        dampingRatio = damping;
    }

    public void SetHalfLife(float halfLife)
    {
        this.halflife = halfLife;
    }


    public float Update(float deltaTime)
    {
        var result = spring_damper_exact_ratio(Value, Velocity, GoalValue, 0f, dampingRatio, halflife, deltaTime);

        Value = result.x;
        Velocity = result.y;

        return result.x;
    }

    public float Predict(float time)
    {
        var result = spring_damper_exact_ratio(Value, Velocity, GoalValue, 0f, dampingRatio, halflife, time);

        return result.x;
    }

    public static Vector2 spring_damper_exact_ratio(float x, float v, float xGoal, float vGoal, float dampingRatio,
        float halflife, float dt, float eps = 1e-5f)
    {
        float d = SpringMath.HalflifeToDamping(halflife);
        float s = SpringMath.DampingRatioToStiffness(dampingRatio, d);
        float c = xGoal + d * vGoal / (s + eps);
        float y = d / 2f;

        if (Mathf.Abs(s - d * d / 4f) < eps)
        {
            // Critically Damped
            float j0 = x - c;
            float j1 = v + j0 * y;

            float eydt = SpringMath.FastNegExp(y * dt);

            x = j0 * eydt + dt * j1 * eydt + c;
            v = -y * j0 * eydt - y * dt * j1 * eydt + j1 * eydt;
        }
        else if (s - d * d / 4f > 0f)
        {
            // Under Damped
            float w = Mathf.Sqrt(s - d * d / 4f);
            float j = Mathf.Sqrt(Mathf.Pow(v + y * (x - c), 2f) / (w * w + eps) + Mathf.Pow(x - c, 2f));
            float p = SpringMath.FastAtan((v + (x - c) * y) / (-(x - c) * w + eps));

            j = x - c > 0f ? j : -j;

            float eydt = SpringMath.FastNegExp(y * dt);

            x = j * eydt * Mathf.Cos(w * dt + p) + c;
            v = -y * j * eydt * Mathf.Cos(w * dt + p) - w * j * eydt * Mathf.Sin(w * dt + p);
        }
        else if (s - d * d / 4f < 0f)
        {
            // Over Damped
            float y0 = (d + Mathf.Sqrt(d * d - 4f * s)) / 2f;
            float y1 = (d - Mathf.Sqrt(d * d - 4f * s)) / 2f;
            float j1 = (c * y0 - x * y0 - v) / (y1 - y0);
            float j0 = x - j1 - c;

            float ey0dt = SpringMath.FastNegExp(y0 * dt);
            float ey1dt = SpringMath.FastNegExp(y1 * dt);

            x = j0 * ey0dt + j1 * ey1dt + c;
            v = -y0 * j0 * ey0dt - y1 * j1 * ey1dt;
        }

        return new Vector2(x, v);
    }
}