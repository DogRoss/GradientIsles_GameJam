using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public interface IGrabbableObject
{
    public bool CanBePickedUp { get; set; }
    public abstract void Pickup(PickupObject pickerUpper);
    public abstract void Place(PickupObject pickerUpper);
    public abstract void AimAtUpdate(PickupObject pickerUpper);
    public abstract void AimAtFixedUpdate(PickupObject pickerUpper);
    public abstract void UpdatePickedUpObject(PickupObject pickerUpper);
    public abstract void FixedUpdatePickedUpObject(PickupObject pickerUpper);
}
