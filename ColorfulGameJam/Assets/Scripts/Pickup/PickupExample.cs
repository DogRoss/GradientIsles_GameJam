using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public class PickupExample : MonoBehaviour, IGrabbableObject
{
    [HideInInspector]
    public TextMesh tmp;

    [SerializeField]
    public Outline outline;

    public bool CanBePickedUp { get; set; } = true;

    public void Awake()
    {
        tmp = GetComponentInChildren<TextMesh>();
        if (tmp != null)
        {
            tmp.text = ":)";
        }
        else
        {
            Debug.LogWarning("uh oh");
        }

        outline = GetComponentInChildren<Outline>();
    }

    public bool outlineEnabled = false;

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
        Debug.Log("I've been picked up by: " + pickerUpper.name);
        if (tmp != null)
            tmp.text = ">:(";
    }

    public void Place(PickupObject pickerUpper)
    {
        Debug.Log("I've been placed by: " + pickerUpper.name);
        if (tmp != null)
            tmp.text = ":)";
    }


    public void UpdatePickedUpObject(PickupObject pickerUpper)
    {
    }

    Vector3 offset = new Vector3();
    int i = 0;
    public void FixedUpdatePickedUpObject(PickupObject pickerUpper)
    {
        if (i % 2 == 0)
        {
            transform.position += offset;
        }
        else
        {
            transform.position -= offset;
            offset = new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), Random.Range(-0.01f, 0.01f));
        }
        i++;
    }
}
