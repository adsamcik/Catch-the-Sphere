using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class GlobalManager {
    public static BonusManager bonusManager = new BonusManager();

    //reflection intensity
    static readonly float reflectionIntensity;
    //ambient light
    static readonly Color ambientLight;
    //sun light
    static readonly Color sunLight;

    public static readonly Light sun;

    static readonly Lock<Color> sunLightLock;
    static readonly Lock<Color> ambientLightLock;
    static readonly Lock<float> reflectionIntensityLock;

    static Comparison<Color> colorComparison = (Color x, Color y) => {
        float val = Mathf.RoundToInt(((x.r + x.g + x.b) / 3) - ((y.r + y.g + y.b) / 3));
        return val > 0 ? Mathf.CeilToInt(val) : Mathf.FloorToInt(val);
    };

    static GlobalManager() {
        sun = GameObject.Find("SUN").GetComponent<Light>();
        ambientLight = RenderSettings.ambientLight;
        ambientLightLock = new Lock<Color>(ambientLight, colorComparison);
        sunLight = sun.color;
        sunLightLock = new Lock<Color>(sunLight, colorComparison);
        reflectionIntensity = RenderSettings.reflectionIntensity;
        reflectionIntensityLock = new Lock<float>(reflectionIntensity, (float x, float y) => { float val = x - y; return val > 0 ? Mathf.CeilToInt(val) : Mathf.FloorToInt(val); });
    }

    public static IEnumerator LightLerp(Light light, Color targetColor, float time) {
        Color original = light.color;
        float value = 0;
        while ((value += Time.deltaTime / time) < 1) {
            light.color = Color.Lerp(original, targetColor, value);
            yield return new WaitForEndOfFrame();
        }
        light.color = targetColor;
    }

    static IEnumerator AmbientLerp(Color targetColor, float time) {
        Color original = RenderSettings.ambientLight;
        float value = 0;
        while ((value += Time.deltaTime / time) < 1) {
            RenderSettings.ambientLight = Color.Lerp(original, targetColor, value * value);
            yield return new WaitForEndOfFrame();
        }
        RenderSettings.ambientLight = targetColor;
    }

    static IEnumerator ReflectionLerp(float target, float time) {
        float original = RenderSettings.reflectionIntensity;
        float value = 0;
        float diff = (target - original);
        while ((value += Time.deltaTime / time) < 1) {
            RenderSettings.reflectionIntensity = original + (diff * value);
            yield return new WaitForEndOfFrame();
        }
        RenderSettings.reflectionIntensity = target;
    }

    public static void SetAmbientLight(Color color, float length) {
        if (ambientLightLock.Add(color))
            GameController.instance.StartCoroutine(AmbientLerp(color, 0.5f));
    }

    public static void ReleaseAmbientLight(Color color) {
        Color c = new Color();
        if (ambientLightLock.Remove(color, ref c))
            GameController.instance.StartCoroutine(AmbientLerp(c, 0.5f));
    }

    public static void SetSunLight(Color color, float length) {
        if (sunLightLock.Add(color))
            GameController.instance.StartCoroutine(LightLerp(sun, color, 0.5f));
    }

    public static void ReleaseSunLight(Color color) {
        Color c = new Color();
        if (sunLightLock.Remove(color, ref c))
            GameController.instance.StartCoroutine(LightLerp(sun, c, 0.5f));
    }
    
    public static void SetReflectionIntensity(float value, float length) {
        if (reflectionIntensityLock.Add(value))
            GameController.instance.StartCoroutine(ReflectionLerp(value, length));
    }

    public static void ReleaseReflectionIntensity(float value) {
        float f = 0;
        if (reflectionIntensityLock.Remove(value, ref f))
            GameController.instance.StartCoroutine(ReflectionLerp(f, 0.5f));
    }



    public class Lock<T> {
        private readonly T original;
        private List<T> active = new List<T>();
        private Comparison<T> comparator;

        public Lock(T original, Comparison<T> comparator) {
            this.original = original;
            this.comparator = comparator;
        }

        public bool Add(T value) {
            active.Add(value);
            if (comparator == null)
                active.Sort();
            else
                active.Sort(comparator);
            return active[0].Equals(value);
        }

        public bool Remove(T value, ref T output) {
            int index = active.IndexOf(value);
            if (index >= 0) {
                if (active.Count == 1)
                    output = original;
                else {
                    if (index == 0)
                        output = active[1];
                    else
                        return false;
                }
                active.RemoveAt(index);
                return true;
            }
            return false;
        }
    }

}
