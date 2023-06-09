using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * 
 * Author: Justin Scruggs
 * 
 * This script can be assigned to any object to pick up objects.
 * Objects that can be picked up need to be given the proper component. (ICanPickThisUp)
 * 
 * InputSystem needs an input named Pickup. This script uses a method named OnPickup.
 * 
 */
public class PickupObject : MonoBehaviour
{
    [Header("Held Object Data")]
    //! The object the player picks up. Can be null.
    [HideInInspector]
    public GameObject pickedUpObject;

    //! The held object's parent transform. Useful for animations and UI.
    [SerializeField]
    public Transform heldObjectTransform;

    [Header("Configuration")]
    [SerializeField]
    [Range(0f, 100f)]
    public float pickupDistance = 5f;

    //! The held object's data. This is to maintain rotation properly and such
    [HideInInspector]
    private Transform heldObjectData;

    [HideInInspector]
    private RigidbodyConstraints heldObjectRigidbodyConstraints;

    [Header("Debug Info")]
    public bool debugInfo = false;

    // This works for placing and picking up objects.
    public void OnPickup(InputValue value)
    {
        if (pickedUpObject == null)
        {
            Pickup();
        }
        else
        {
            Place();
        }
    }

    void Pickup()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance);

        if (debugInfo)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward.normalized * pickupDistance, Color.green);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.name + ", has canpickup script? " + (hit.collider.GetComponent<IGrabbableObject>() != null));
                }
            }
        }

        if (hits.Length <= 0)
            return;

        RaycastHit h = hits[0];
        IGrabbableObject commandGrab = null; // >:)
        foreach (RaycastHit hit in hits)
        {
            IGrabbableObject g = hit.collider.GetComponent<IGrabbableObject>();
            if (g != null)
            {
                if (!g.CanBePickedUp)
                    continue;
                commandGrab = g;
                h = hit;
                break;
            }
        }

        if (commandGrab == null)
            return;

        Collider c = h.collider;
        Rigidbody rb = c.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Freeze all the rigidbody constraints and store the original constraints
            heldObjectRigidbodyConstraints = rb.constraints;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.detectCollisions = false;
        }
        // Disable the collider to prevent pushing self
        c.enabled = false;

        // copy the data to set it later
        heldObjectData = c.transform;

        // set the object's parent transform to heldObjectTransform so it looks like we're holding it (if its not null)
        if (heldObjectTransform != null)
        {
            c.transform.parent = heldObjectTransform;
        }
        else
        {
            c.transform.parent = transform;
            c.transform.position = transform.position + transform.forward * 2.5f;
            c.transform.LookAt(transform);
        }

        pickedUpObject = c.gameObject;
        //Debug.Log(commandGrab);
        commandGrab.Pickup(this);
    }

    void Place()
    {
        // check if we're colliding with anything
        Collider c = pickedUpObject.GetComponent<Collider>();

        // hell
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, transform.forward, pickupDistance * 1.1f);
        if (hits.Length != 0)
        {
            float dist = float.MaxValue; // idk !!
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.isTrigger)
                    continue;

                if (hit.distance < dist)
                {
                    dist = hit.distance;
                }
            }
        }

        RaycastHit[] aimAtHits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance * 1.1f);
        RaycastHit aimAtNearest = new RaycastHit();
        if (aimAtHits.Length != 0)
        {
            float dist = float.MaxValue; // idk !!
            foreach (RaycastHit hit in aimAtHits)
            {
                if (hit.collider.isTrigger)
                    continue;

                if (hit.distance < dist)
                {
                    dist = hit.distance;
                    aimAtNearest = hit;
                }
            }

            //   v if we have 0 colliders (needed to avoid an error?)
            if (aimAtNearest.collider == null || aimAtNearest.collider.GetComponent<IGrabbableHolder>() == null) // if its not over a holder, check to return or not
            {
                if (Vector3.Distance(pickedUpObject.transform.position, Camera.main.transform.position) >= dist)
                    return;
            }
        }

        ReplaceObjectData();

        // holder 
        if (aimAtNearest.collider != null)
        {
            //Debug.Log("nearest collider exists");
            IGrabbableHolder a = aimAtNearest.collider.GetComponent<IGrabbableHolder>();
            if (a != null)
            {
                //Debug.Log("we can place on it");
                a.PlaceOn(this, pickedUpObject);
            }
        }

        IGrabbableObject igo = pickedUpObject.GetComponent<IGrabbableObject>();
        if (igo != null)
        {
            igo.Place(this);
        }

        pickedUpObject = null;
    }

    private void ReplaceObjectData()
    {
        // set picked up object's transform data back
        pickedUpObject.transform.parent = null;

        // update the position to be in front of the player
        pickedUpObject.transform.position = heldObjectData.position;
        pickedUpObject.transform.rotation = heldObjectData.rotation;
        pickedUpObject.transform.localScale = heldObjectData.localScale;

        // reenable object's colliders & rigidbody things if they exist
        Rigidbody rb = pickedUpObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Freeze all the rigidbody constraints and store the original constraints
            rb.constraints = heldObjectRigidbodyConstraints;
            rb.detectCollisions = true;
        }

        // hacky
        pickedUpObject.GetComponent<Collider>().enabled = true;
        pickedUpObject.transform.position = transform.position + transform.forward * 2.5f;
    }

    private void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance);
        IGrabbableHolder holder = null;
        foreach (RaycastHit h in hits)
        {
            if (h.collider.isTrigger)
                continue;
            holder = h.collider.GetComponent<IGrabbableHolder>();
            if (holder != null) // optimization
                break;
        }

        RaycastHit[] grabbables = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance);
        IGrabbableObject grabbable = null;
        foreach (RaycastHit h in grabbables)
        {
            if (h.collider.isTrigger)
                continue;
            grabbable = h.collider.GetComponent<IGrabbableObject>();
            if (grabbable != null && !grabbable.CanBePickedUp) // If the object can no longer be picked up, we ignore the object and just pretend we didnt hit it
                grabbable = null;
            if (grabbable != null) // optimization
                break;
        }

        if (pickedUpObject != null)
        {
            IGrabbableObject igo = pickedUpObject.GetComponent<IGrabbableObject>();
            igo.UpdatePickedUpObject(this);

            if (holder != null)
            {
                holder.AimAtUpdateWhenHeld(this, pickedUpObject);
            }
        }

        if (grabbable != null)
        {
            grabbable.AimAtUpdate(this);
        }

        if (holder != null)
        {
            holder.AimAtUpdate(this);
        }
    }

    private void FixedUpdate()
    {
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance);
        IGrabbableHolder holder = null;
        foreach (RaycastHit h in hits)
        {
            if (h.collider.isTrigger)
                continue;
            holder = h.collider.GetComponent<IGrabbableHolder>();
            if (holder != null) // optimization
                break;
        }

        RaycastHit[] grabbables = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward, pickupDistance);
        IGrabbableObject grabbable = null;
        foreach (RaycastHit h in grabbables)
        {
            if (h.collider.isTrigger)
                continue;
            grabbable = h.collider.GetComponent<IGrabbableObject>();
            if (grabbable != null && !grabbable.CanBePickedUp) // If the object can no longer be picked up, we ignore the object and just pretend we didnt hit it
                grabbable = null;
            if (grabbable != null) // optimization
                break;
        }

        if (pickedUpObject != null)
        {
            IGrabbableObject igo = pickedUpObject.GetComponent<IGrabbableObject>();
            igo.FixedUpdatePickedUpObject(this);

            if (holder != null)
            {
                holder.AimAtFixedUpdateWhenHeld(this, pickedUpObject);
            }
        }

        if (holder != null)
        {
            holder.AimAtFixedUpdate(this);
        }

        if (grabbable != null)
        {
            grabbable.AimAtFixedUpdate(this);
        }
    }
}
