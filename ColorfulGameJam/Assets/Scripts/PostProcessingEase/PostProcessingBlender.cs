using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/*
 * Author: Justin Scruggs
 *  
 * Ease a post processing volume's values.
 * this script was made with PURE, UNBRIDLED HATRED towards post processing values @ 10:00 am
 * working with this is a nightmare.. please never do this to yourself @ 10:15 am
 * this is easily the best-possible-damage-control code i've ever written @ 10:30 am
 * i am now regretting the above line @ 11:53 am
 * 12:46pm i have deleted the ENTIRE class !!!
 * 
 * 2:13 after coming back from lunch i have finished it in 13 minutes
 * 
 */
public class PostProcessingBlender : MonoBehaviour
{
    [HideInInspector]
    PostProcessVolume volume1;
    [HideInInspector]
    PostProcessVolume volume2;

    private void Awake()
    {
        PostProcessVolume[] volumes = GetComponentsInChildren<PostProcessVolume>();
        if (volumes.Length != 2)
        {
            Debug.LogError("More/Less than 2 volumes on the player.");
        } else
        {
            volume1 = volumes[0];
            volume2 = volumes[1];
        }
    }

    private static Coroutine cachedCoroutine;

    public void RunBlend(PostProcessProfile profile, float lerpTime)
    {
        Debug.Log("Attempting to run blend, running? " + (cachedCoroutine == null));
        if (cachedCoroutine == null)
            cachedCoroutine = StartCoroutine(LerpVolume(profile, lerpTime));
    }

    private IEnumerator LerpVolume(PostProcessProfile profile, float lerpTime)
    {
        Debug.Log("blending start");
        volume1.weight = 1;
        volume2.profile = profile;
        float startTime = Time.time;
        while (Time.time < lerpTime + startTime)
        {
            float percent = (Time.time - startTime) / lerpTime;

            volume2.weight = percent;

            yield return null;
        }

        volume2.weight = 0;
        volume1.profile = profile;

        Debug.Log("blending done");
        cachedCoroutine = null;
    }
}
