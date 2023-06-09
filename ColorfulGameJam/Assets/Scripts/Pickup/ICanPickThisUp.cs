using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public class ICanPickThisUp : MonoBehaviour, IGrabbableObject
{
    // just add this as a component if you want to pick this up this object

    public bool CanBePickedUp { get; set; } = true;

    public void Pickup(PickupObject _)
    {

    }

    public void Place(PickupObject _)
    {

    }

    public void UpdatePickedUpObject(PickupObject _)
    {

    }

    public void FixedUpdatePickedUpObject(PickupObject _)
    {

    }

    public void AimAtUpdate(PickupObject _)
    {

    }

    public void AimAtFixedUpdate(PickupObject _)
    {

    }
}
