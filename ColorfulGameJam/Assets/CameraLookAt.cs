using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CameraLookAt : MonoBehaviour
{
    [Header("Needed Objects")]
    public GameObject LookAtTarget;
    public GameObject totem;
    public SplineWalker walker;
    public BezierSpline Nextspline;
    public Image fadeImage;


    [Header("Adjustments")]
    public float Speed = 1;

    Color totemColor;
    int lapCount;

    bool transition = false;

    IEnumerator FadeIN()
    {
        float alpha = fadeImage.color.a;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * Speed;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            yield return null;
        }

        yield return null;
    }

    IEnumerator FadeOut()
    {
        float alphat = fadeImage.color.a;

        while (alphat > 0)
        {
            alphat -= Time.deltaTime * Speed;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alphat);

            yield return null;
        }

        yield return null;
    }

    IEnumerator TotemFadeOut()
    {
        float alphat = totem.GetComponent<MeshRenderer>().material.color.a;
        yield return new WaitForSeconds(3f);
        while (alphat > 0)
        {
            alphat -= Time.deltaTime * Speed;
            totem.GetComponent<MeshRenderer>().material.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alphat);

            yield return null;
        }

        yield return null;
    }

    IEnumerator FinalFadeIn()
    {
        yield return new WaitForSeconds(5f);
        float alpha = fadeImage.color.a;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * Speed;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            yield return null;
        }


        LoadSceneAsync.instance.LoadScene(3);

    }

    IEnumerator CinematicFadeIn()
    {
        float alpha = fadeImage.color.a;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * Speed;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, alpha);

            yield return null;
        }
        LoadSceneAsync.instance.LoadScene(0);
    }



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        if (!transition)
        {
            transform.LookAt(LookAtTarget.transform, Vector3.up);
        }
        else
        {
            transform.LookAt(totem.transform, Vector3.up);
        }

        if (lapCount == 2)
        {
            StartCoroutine(CinematicFadeIn());
            lapCount++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            StartCoroutine(FadeIN());
            Debug.Log("Working");
        }

        if (other.gameObject.CompareTag("Respawn"))
        {
            transition = true;
            walker.spline = Nextspline;
            Debug.Log("Change Position");
            StartCoroutine(FadeOut());
            StartCoroutine(TotemFadeOut());
            StartCoroutine(FinalFadeIn());

        }

        if (other.gameObject.name == "LapCount")
        {
            lapCount++;
            Debug.Log("LapHit");
        }

    }
}
