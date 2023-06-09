using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// *
/// * Script by Edgar
/// *
/// * Camera screen shake for certain interactions;
/// *
/// </summary>
public class ScreenShake : MonoBehaviour
{
    [SerializeField] Camera cam;

    [Header("Shake VB")]
    float shake = 0f;
    [SerializeField] float shakeAmount = 0.5f;
    protected bool isShaking = false;

    // Update is called once per frame
    void Update()
    {
        if(shake > 0)
        {
            cam.transform.localPosition = Random.insideUnitSphere * shakeAmount;
            shake -= Time.deltaTime;
            isShaking = true;
        }
        else
        {
            shake = 0;
            isShaking = false;
        }
    }

    public void ShakeScreen(float seconds)
    {
        shake = seconds;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        shake = 50;
    }

    public bool IsShaking()
    {
        return isShaking;
    }
}
