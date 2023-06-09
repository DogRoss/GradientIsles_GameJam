using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * Author: Justin Scruggs
 * 
 */
public interface IGrabbableHolder
{
    public abstract void AimAtFixedUpdate(PickupObject pickerUpper);
    public abstract void AimAtUpdate(PickupObject pickerUpper);
    public abstract void AimAtFixedUpdateWhenHeld(PickupObject pickerUpper, GameObject thing);
    public abstract void AimAtUpdateWhenHeld(PickupObject pickerUpper, GameObject thing);
    public abstract void PlaceOn(PickupObject pickerUpper, GameObject thing);
    public abstract bool CanPlaceOn(PickupObject pickerUpper, GameObject thing);
}
