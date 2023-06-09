using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPlaced : MonoBehaviour
{
    public IslandScript island;
    public ScreenShake playersShakeScript;

    [Header("Material needs to be fade")]
    public GameObject fadeObject;

    [Header("Adjustments")]
    [SerializeField] float fadeSpeed;

    Color ObjectColor;

    // Start is called before the first frame update
    void Start()
    {
        ObjectColor = fadeObject.GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Totem"))
        {
            StartCoroutine(ObjectFadeOut());
            island.isTotemPlaced = true;
            playersShakeScript.Play();
            Debug.Log("Working");
        }
    }

    IEnumerator ObjectFadeOut()
    {
        float alphat = fadeObject.GetComponent<MeshRenderer>().material.color.a;
        yield return new WaitForSeconds(3f);
        while (alphat > 0)
        {
            alphat -= Time.deltaTime * fadeSpeed;
            fadeObject.GetComponent<MeshRenderer>().material.color = new Color(ObjectColor.r, ObjectColor.g, ObjectColor.b, alphat);
            yield return null;
        }
        if (alphat <= 0)
        {
            fadeObject.SetActive(false);
        }

        yield return null;
    }


}
