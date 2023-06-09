using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public class Altar : MonoBehaviour, IGrabbableHolder
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
        }
    }

    public bool CanPlaceOn(PickupObject pickerUpper, GameObject thing)
    {
        return thing.GetComponent<Totem>() != null;
    }
}
