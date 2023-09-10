using UnityEngine;

namespace MathUtils
{
    public static class SpringMath
    {
        public static float HalflifeToDamping(float halflife)
        {
            return 4f * 0.69314718056f / (halflife + 1e-5f);
        }

        public static float DampingRatioToStiffness(float ratio, float damping)
        {
            return Square(damping / (ratio * 2f));
        }

        public static float Square(float val)
        {
            return val * val;
        }

        public static float FastNegExp(float x)
        {
            return 1f / (1f + x + 0.48f * x * x + 0.235f * x * x * x);
        }

        public static float Copysign(float a, float b)
        {
            if (b <= 0.01f && b >= -0.01f) return 0f;

            return Mathf.Abs(a) * Mathf.Sign(b);
        }

        public static float FastAtan(float x)
        {
            var z = Mathf.Abs(x);
            var w = z > 1f ? 1f / z : z;
            var y = Mathf.PI / 4f * w - w * (w - 1f) * (0.2447f + 0.0663f * w);
            return Copysign(z > 1f ? Mathf.PI / 2f - y : y, x);
        }
    }
}