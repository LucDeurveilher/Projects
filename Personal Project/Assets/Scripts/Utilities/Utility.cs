using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public static class Utility
{
    public static void Shuffle<T>(List<T> list)
    {
        System.Random rand = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }

    public static IEnumerator FadeIn(CanvasGroup group, float alpha, float duration)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
        yield return null;
    }
    public static IEnumerator FadeIn(CanvasGroup group, float alpha, float duration, Action onComplete = null)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;

        onComplete?.Invoke();
        yield return null;
    }

    public static IEnumerator FadeOut(CanvasGroup group, float alpha, float duration)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
        yield return null;
    }
    public static IEnumerator FadeOut(CanvasGroup group, float alpha, float duration, Action onComplete = null)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;

        while (time < duration)
        {
            time += Time.deltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;

        onComplete?.Invoke();
        yield return null;
    }

    public static IEnumerator FadeOutGameObject(GameObject obj, float alpha, float duration)
    {
        SpriteRenderer[] renderer = obj.GetComponentsInChildren<SpriteRenderer>();

        var time = 0.0f;
        var originalAlpha = 1;

        while (time < duration)
        {
            time += Time.deltaTime;
            foreach (SpriteRenderer spriteRenderer in renderer)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(originalAlpha, alpha, time / duration);
                spriteRenderer.color = color;
            }
            yield return new WaitForEndOfFrame();
        }

        foreach (SpriteRenderer spriteRenderer in renderer)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        yield return null;
    }

    public static IEnumerator TranslateGameObject(GameObject obj, Transform end, float duration, Action onComplete = null)
    {
        var time = 0.0f;
        var originalPosition = obj.transform.position;

        Vector3 endPos = end.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            obj.transform.position = Vector3.Lerp(originalPosition, endPos, time / duration);
            yield return new WaitForEndOfFrame();
        }

        obj.transform.position = end.position;
        onComplete?.Invoke();

        yield return null;

    }

    //Particular fonction 
    public static IEnumerator ReturnTrip(GameObject obj, Transform end, float duration, Action onComplete = null, float waitMiddle = 0, Func<float, float> easingFunction = null)
    {
        var time = 0.0f;
        var originalPosition = obj.transform.position;

        Vector3 endPos = end.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            obj.transform.position = Easing.EasingVector(originalPosition, endPos, time / duration, easingFunction);
            yield return new WaitForEndOfFrame();
        }

        obj.transform.position = end.position;
        onComplete?.Invoke();

        yield return new WaitForSeconds(waitMiddle);


        time = 0.0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            obj.transform.position = Easing.EasingVector(endPos, originalPosition, time / duration, easingFunction);
            yield return new WaitForEndOfFrame();
        }

        obj.transform.position = originalPosition;


        yield return null;

    }

    public static IEnumerator TranslateLocalPos(GameObject _origin, Vector3 _endPos, float duration, Func<float, float> easingFunction = null,Action onComplete = null)
    {
        float time = 0.0f;
        Vector3 originalPosition = _origin.transform.localPosition;

        while (time < duration)
        {
            time += Time.deltaTime;
            if (easingFunction != null)
            {
                _origin.transform.localPosition = Easing.EasingVector(originalPosition, _endPos, time / duration, easingFunction);
            }
            else
            {
                _origin.transform.localPosition = Vector3.Lerp(originalPosition, _endPos, time / duration);
            }
            yield return new WaitForEndOfFrame();
        }
        onComplete?.Invoke();
        _origin.transform.localPosition = _endPos;
        yield return null;
    }

    public static IEnumerator PlayFonctionAfterTimer(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);

        onComplete?.Invoke();

        yield return null;
    }

    public static void FlipSprite(GameObject obj, bool flipX, bool flipY)
    {
        Vector3 scale = obj.transform.localScale;
        if (flipX)
            scale.x *= -1;
        if (flipY)
            scale.y *= -1;

        obj.transform.localScale = scale;
    }

    public static bool CheckValueBetween(float value, float min, float max)
    {
        if (value < min || value > max)
        {
            return false;
        }

        return true;
    }
}
