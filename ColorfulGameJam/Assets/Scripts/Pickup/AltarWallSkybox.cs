using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarWallSkybox : MonoBehaviour, IGrabbableHolder
{
    [Header("Configuration")]
    [Tooltip("The point where the totem is placed on.")]
    [SerializeField]
    Transform placePoint;

    [HideInInspector]
    public bool hasObject = false;

    [HideInInspector]
    private Outline outline;

    [HideInInspector]
    private bool outlineEnabled = false;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        if (placePoint == null)
        {
            Debug.LogError("PlacePoint in Altar is null. Please assign a Transform.");
        }
    }

    public void AimAtUpdate(PickupObject pickerUpper)
    {
    }
    public void AimAtFixedUpdate(PickupObject pickerUpper)
    {

    }

    public void FixedUpdate()
    {
        if (outline != null)
        {
            if (outlineEnabled)
            {
                if (!outline.enabled)
                    outline.enabled = true;
            }
            else
            {
                if (outline.enabled)
                    outline.enabled = false;
            }
            outlineEnabled = false;
        }
    }

    public void AimAtUpdateWhenHeld(PickupObject pickerUpper, GameObject thing)
    {
    }
    public void AimAtFixedUpdateWhenHeld(PickupObject pickerUpper, GameObject thing)
    {
        if (hasObject)
            return;
        if (!CanPlaceOn(pickerUpper, thing))
            return;
        if (outline != null)
        {
            outlineEnabled = true;
        }
    }

    public void PlaceOn(PickupObject pickerUpper, GameObject thing)
    {
        if (placePoint == null)
            return;
        if (!hasObject && CanPlaceOn(pickerUpper, thing))
        {
            thing.transform.SetPositionAndRotation(placePoint.position, placePoint.rotation);
            thing.GetComponent<IGrabbableObject>().CanBePickedUp = false;
            hasObject = true;
            Rigidbody rb = thing.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
            DoFadeOut();
        }
    }

    public void DoFadeOut()
    {
        StartCoroutine(ObjectFadeOut());
        playersShakeScript.ShakeScreen(3f);
    }

    public bool CanPlaceOn(PickupObject pickerUpper, GameObject thing)
    {
        return thing.GetComponent<Totem>() != null;
    }

    [SerializeField]
    public ScreenShake playersShakeScript;

    [SerializeField]
    public GameObject fadeObject;

    [SerializeField]
    public float fadeSpeed;
    
    [SerializeField]
    public Color ObjectColor;

    IEnumerator ObjectFadeOut()
    {
        float alphat = fadeObject.GetComponent<MeshRenderer>().material.color.a;
        yield return new WaitForSeconds(1f);
        while (alphat > 0)
        {
            alphat -= Time.deltaTime * fadeSpeed;
            fadeObject.GetComponent<MeshRenderer>().material.color = new Color(ObjectColor.r, ObjectColor.g, ObjectColor.b, alphat);
            yield return null;
        }
        fadeObject.SetActive(false);

        yield return null;
    }
}
