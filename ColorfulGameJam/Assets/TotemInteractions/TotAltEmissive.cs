using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotAltEmissive : MonoBehaviour
{
    [Header("Game Objects")]
    [SerializeField] PickupObject grabbableObject;
    [SerializeField] Transform playerCam;

    [Header("Particle System")]
    [SerializeField] ParticleSystem pSystem;

    [Header("Distance Info")]
    public bool withinRange = false;
    public float checkRadius = 20f;
    Vector3 dirAtoB = Vector3.zero;
    float dot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (pSystem.isPlaying)
            pSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {

        //checks if the player has the totem item and is within range
        if(withinRange && grabbableObject != null && grabbableObject.CompareTag("Totem"))
        //if(withinRange)
        {
            //checks if the player is looking at the altar
            dirAtoB = (transform.position - playerCam.position).normalized;
            dot = Vector3.Dot(dirAtoB, playerCam.forward);
            //if mostly looking towards altar
            if (dot > 0.9) 
            {
                if(!pSystem.isPlaying)
                    pSystem.Play();
            }
            else
            {
                if (pSystem.isPlaying)
                    pSystem.Stop();
            }
        }
        else
        {
            if (pSystem.isPlaying)
                pSystem.Stop();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            withinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            withinRange = false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, checkRadius);
    //}
}
