using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/*
 * Author: Justin Scruggs
 * Apply this to a trigger to change the post processing profile on the camera.
 */
public class PostProcessingTrigger : MonoBehaviour
{
    [DisplayName("Profile to Blend")]
    public PostProcessProfile profile;

    [DisplayName("Time to Blend")]
    [Range(0, 100)]
    public float lerpTime = 10f;

    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<FirstPersonMovementRB>();
        if (player != null)
        {
            var blender = other.GetComponentInChildren<PostProcessingBlender>();
            if (blender != null)
            {
                blender.RunBlend(profile, lerpTime);
            }
        }
    }
}
