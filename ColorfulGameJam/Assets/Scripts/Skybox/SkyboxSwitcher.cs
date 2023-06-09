using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Justin Scruggs
 *  
 * """Simple""" script for changing the skybox of the scene.
 * this script is made with black magic and 2 shots of espresso
 * 
 */
[RequireComponent(typeof(Collider))]
public class SkyboxSwitcher : MonoBehaviour
{
    [Header("Configuration")]

    [SerializeField]
    [Tooltip("When entering the trigger, the skybox will start at this material.")]
    // The material to change the skybox from.
    public Material changeFrom;

    [Tooltip("When entering the trigger, the skybox will change to this material.")]
    [SerializeField]
    // The material to change the skybox to.
    public Material changeTo;

    [SerializeField]
    [Range(0f, 100f)]
    [Tooltip("How long it takes to animate the skybox change.")]
    public float lerpTime = 10f;

    [SerializeField]
    [Tooltip("Force the change. This will change the skybox disregarding if the current skybox is the one to change from.")]
    public bool forceChange = false;

    [HideInInspector]
    private Material lerpMat;

    // Colors and floats to lerp. Cached for performance.
    // good lord

    // each variable needs a start variable which comes from changeFrom
    // then a new variable which starts at the variable
    // then an end variable which comes from changeTo

    struct FloatContainer
    {
        public float start;
        public float update;
        public float end;
        public string property;
    }

    struct ColorContainer
    {
        public Color start;
        public Color update;
        public Color end;
        public string property;
    }

    FloatContainer sunDisk;
    FloatContainer sunSize;
    FloatContainer sunSizeConvergence;
    FloatContainer atmosphereThickness;
    ColorContainer skyTint;
    ColorContainer groundColor;
    FloatContainer exposure;

    // less agony

    private readonly static string SUN_DISK = "_SunDisk"; // Float
    private readonly static string SUN_SIZE = "_SunSize"; // Float/Range
    private readonly static string SUN_SIZE_CONVERGENCE = "_SunSizeConvergence"; // Float/Range
    private readonly static string ATMOSPHERE_THICKNESS = "_AtmosphereThickness"; // Float/Range
    private readonly static string SKY_TINT = "_SkyTint"; // Color
    private readonly static string GROUND_COLOR = "_GroundColor"; // Color
    private readonly static string EXPOSURE = "_Exposure"; // Float

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        lerpMat = new Material(changeFrom)
        {
            name = "MAT_LerpingSkybox-" + changeFrom.name + "-" + changeTo.name
        };
        lerpMat.CopyPropertiesFromMaterial(changeFrom);

        sunDisk = InitializeFloatContainer(sunDisk, SUN_DISK);
        sunSize = InitializeFloatContainer(sunSize, SUN_SIZE);
        sunSizeConvergence = InitializeFloatContainer(sunSizeConvergence, SUN_SIZE_CONVERGENCE);
        atmosphereThickness = InitializeFloatContainer(atmosphereThickness, ATMOSPHERE_THICKNESS);
        skyTint = InitializeColorContainer(skyTint, SKY_TINT);
        groundColor = InitializeColorContainer(groundColor, GROUND_COLOR);
        exposure = InitializeFloatContainer(exposure, EXPOSURE);
    }

    private FloatContainer InitializeFloatContainer(FloatContainer fc, string property)
    {
        fc.property = property;
        fc.start = changeFrom.GetFloat(fc.property);
        fc.update = fc.start;
        fc.end = changeTo.GetFloat(fc.property);

        return fc;
    }

    private ColorContainer InitializeColorContainer(ColorContainer cc, string property)
    {
        cc.property = property;
        cc.start = changeFrom.GetColor(cc.property);
        cc.update = cc.start;
        cc.end = changeTo.GetColor(cc.property);

        return cc;
    }

    private static Coroutine cachedCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        var player = other.GetComponent<FirstPersonMovementRB>();
        if (player != null)
        {
            if (!forceChange && RenderSettings.skybox != changeFrom)
                return;
            if (cachedCoroutine == null)
                cachedCoroutine = StartCoroutine(LerpSkybox());
        }
    }

    private IEnumerator LerpSkybox()
    {
        Initialize();

        Debug.Log("Starting to Lerp Skybox, " + changeFrom.name + " to " + changeTo.name);

        RenderSettings.skybox = lerpMat;

        float startTime = Time.time;
        while (Time.time < lerpTime + startTime)
        {
            float percent = (Time.time - startTime) / lerpTime;
            //Debug.Log(percent);

            // Lerp Functions
            sunDisk = LerpFloatContainer(sunDisk, percent, lerpMat);
            sunSize = LerpFloatContainer(sunSize, percent, lerpMat);
            sunSizeConvergence = LerpFloatContainer(sunSizeConvergence, percent, lerpMat);
            atmosphereThickness = LerpFloatContainer(atmosphereThickness, percent, lerpMat);
            skyTint = LerpColorContainer(skyTint, percent, lerpMat);
            groundColor = LerpColorContainer(groundColor, percent, lerpMat);
            exposure = LerpFloatContainer(exposure, percent, lerpMat);

            yield return null;
        }

        RenderSettings.skybox = changeTo;

        Debug.Log("Finished Lerping Skybox, " + changeFrom.name + " to " + changeTo.name);
        cachedCoroutine = null;
    }

    private ColorContainer LerpColorContainer(ColorContainer cc, float percent, Material materialToSet = null)
    {
        cc.update = Color.Lerp(cc.start, cc.end, EaseInOutQuad(percent));
        if (materialToSet != null)
        {
            materialToSet.SetColor(cc.property, cc.update);
        }
        return cc;
    }

    private FloatContainer LerpFloatContainer(FloatContainer fc, float percent, Material materialToSet = null)
    {
        fc.update = Mathf.Lerp(fc.start, fc.end, EaseInOutQuad(percent));
        if (materialToSet != null)
        {
            materialToSet.SetFloat(fc.property, fc.update);
        }
        return fc;
    }

    private float EaseInOutQuad(float x) {
        return x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
    }
}
