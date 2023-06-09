using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public class Totem : MonoBehaviour, IGrabbableObject
{
    [HideInInspector]
    private Outline outline;
    
    private bool outlineEnabled = false;

    public bool CanBePickedUp { get; set; } = true;

    public void Awake()
    {
        outline = GetComponentInChildren<Outline>();
    }

    public void AimAtFixedUpdate(PickupObject pickerUpper)
    {
        if (outline != null)
        {
            outlineEnabled = true;
        }
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

    public void AimAtUpdate(PickupObject pickerUpper)
    {
    }

    public void Pickup(PickupObject pickerUpper)
    {
    }

    public void Place(PickupObject pickerUpper)
    {
    }

    public void UpdatePickedUpObject(PickupObject pickerUpper)
    {
    }

    public void FixedUpdatePickedUpObject(PickupObject pickerUpper)
    {
    }
}
