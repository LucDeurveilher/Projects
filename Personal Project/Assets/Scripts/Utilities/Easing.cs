using System;
using UnityEngine;

public static class Easing
{
    //Basics
    public static float EaseInSine(float x) => (float)(1 - Math.Cos((x * Math.PI) / 2));
    public static float EaseOutSine(float x) => (float)Math.Sin((x * Math.PI) / 2);
    public static float EaseInOutSine(float x) => (float)(-(Math.Cos(Math.PI * x) - 1) / 2);
    public static float EaseInQuad(float x) => x * x;
    public static float EaseOutQuad(float x) => x * (2 - x);
    public static float EaseInOutQuad(float x) => x < 0.5f ? 2 * x * x : 1 - (float)Math.Pow(-2 * x + 2, 2) / 2;
    public static float EaseInCubic(float x) => x * x * x;
    public static float EaseOutCubic(float x) => 1 - (float)Math.Pow(1 - x, 3);
    public static float EaseInOutCubic(float x) => x < 0.5f ? 4 * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 3) / 2;

    // Specifics
    public static float EaseInQuart(float x) => x * x * x * x;
    public static float EaseOutQuart(float x) => 1 - (float)Math.Pow(1 - x, 4);
    public static float EaseInOutQuart(float x) => x < 0.5f ? 8 * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 4) / 2;

    public static float EaseInQuint(float x) => x * x * x * x * x;
    public static float EaseOutQuint(float x) => 1 - (float)Math.Pow(1 - x, 5);
    public static float EaseInOutQuint(float x) => x < 0.5f ? 16 * x * x * x * x * x : 1 - (float)Math.Pow(-2 * x + 2, 5) / 2;

    public static float EaseInExpo(float x) => x == 0 ? 0 : (float)Math.Pow(2, 10 * x - 10);
    public static float EaseOutExpo(float x) => x == 1 ? 1 : 1 - (float)Math.Pow(2, -10 * x);
    public static float EaseInOutExpo(float x) =>
        x == 0 ? 0 : x == 1 ? 1 : x < 0.5 ? (float)Math.Pow(2, 20 * x - 10) / 2 : (2 - (float)Math.Pow(2, -20 * x + 10)) / 2;

    public static float EaseInCirc(float x) => (float)(1 - Math.Sqrt(1 - Math.Pow(x, 2)));
    public static float EaseOutCirc(float x) => (float)Math.Sqrt(1 - Math.Pow(x - 1, 2));
    public static float EaseInOutCirc(float x) =>
        x < 0.5 ? (1 - (float)Math.Sqrt(1 - Math.Pow(2 * x, 2))) / 2 : ((float)Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2;

    public static float EaseInBack(float x) => (float)(2.70158 * x * x * x - 1.70158 * x * x);
    public static float EaseOutBack(float x) => (float)(1 + 2.70158 * Math.Pow(x - 1, 3) + 1.70158 * Math.Pow(x - 1, 2));
    public static float EaseInOutBack(float x) =>
        x < 0.5
            ? (float)(Math.Pow(2 * x, 2) * ((2.70158 + 1) * 2 * x - 2.70158)) / 2
            : (float)((Math.Pow(2 * x - 2, 2) * ((2.70158 + 1) * (x * 2 - 2) + 2.70158) + 2) / 2);

    public static float EaseInElastic(float x) =>
        x == 0 ? 0 : x == 1 ? 1 : -(float)Math.Pow(2, 10 * x - 10) * (float)Math.Sin((x * 10 - 10.75) * ((2 * Math.PI) / 3));
    public static float EaseOutElastic(float x) =>
        x == 0 ? 0 : x == 1 ? 1 : (float)Math.Pow(2, -10 * x) * (float)Math.Sin((x * 10 - 0.75) * ((2 * Math.PI) / 3)) + 1;
    public static float EaseInOutElastic(float x) =>
        x == 0
            ? 0
            : x == 1
                ? 1
                : x < 0.5
                    ? -(float)(Math.Pow(2, 20 * x - 10) * Math.Sin((20 * x - 11.125) * ((2 * Math.PI) / 4.5))) / 2
                    : (float)(Math.Pow(2, -20 * x + 10) * Math.Sin((20 * x - 11.125) * ((2 * Math.PI) / 4.5))) / 2 + 1;

    public static float EaseInBounce(float x) => 1 - EaseOutBounce(1 - x);
    public static float EaseOutBounce(float x)
    {
        if (x < 1 / 2.75f)
            return 7.5625f * x * x;
        else if (x < 2 / 2.75f)
            return 7.5625f * (x -= 1.5f / 2.75f) * x + 0.75f;
        else if (x < 2.5 / 2.75)
            return 7.5625f * (x -= 2.25f / 2.75f) * x + 0.9375f;
        else
            return 7.5625f * (x -= 2.625f / 2.75f) * x + 0.984375f;
    }

    public static float EaseInOutBounce(float x) =>
        x < 0.5 ? (1 - EaseOutBounce(1 - 2 * x)) / 2 : (1 + EaseOutBounce(2 * x - 1)) / 2;

    // BaseEasing et EasingVector (inchangés)
    public static float BaseEasing(float start, float end, float t, Func<float, float> easingFunction)
    {
        t = Mathf.Clamp01(t);
        return start + (end - start) * easingFunction(t);
    }

    public static Vector3 EasingVector(Vector3 start, Vector3 end, float t, Func<float, float> easingFunction)
    {
        t = Mathf.Clamp01(t);
        return new Vector3(
            BaseEasing(start.x, end.x, t, easingFunction),
            BaseEasing(start.y, end.y, t, easingFunction),
            BaseEasing(start.z, end.z, t, easingFunction)
        );
    }
}
