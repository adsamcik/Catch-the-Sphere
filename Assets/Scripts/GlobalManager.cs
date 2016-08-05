using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GlobalManager {
    public static BonusManager bonusManager = new BonusManager();

    //reflection intensity
    static float reflectionIntensity;
    static int riLocks = 0;
    //ambient light
    static Color ambientLight;
    static int alLocks = 0;
    //sun light
    static List<Color> sunLightsColors = new List<Color>();
    static Color sunLight;
    static int slLocks = 0;

    public static Light sun;


    static GlobalManager() {
        sun = GameObject.Find("SUN").GetComponent<Light>();
        sunLight = sun.color;
        ambientLight = RenderSettings.ambientLight;
    }

    public static IEnumerator LightLerp(Light light, Color targetColor, float time) {
        Color original = sun.color;
        float value = 0;
        while ((value += Time.deltaTime) < 1) {
            light.color = Color.Lerp(original, targetColor, value * time);
            yield return new WaitForEndOfFrame();
        }
        light.color = targetColor;
    }

    public static void SetAmbientLightAndLock(SphereController controller, Color color, float length) {
        alLocks++;
        controller.StartCoroutine(LightLerp(sun, ambientLight, 0.5f));
    }

    public static void ReleaseAmbientLight(Color color) {
        if (--alLocks == 0)
            GameController.instance.StartCoroutine(LightLerp(sun, ambientLight, 0.5f));
    }

    class Lock {
        private Color light;
        private Color originalColor;
        private List<Color> activeColors = new List<Color>();

        public static void AddLight(SphereController controller, Color color, float length) {

        }
    }

}
