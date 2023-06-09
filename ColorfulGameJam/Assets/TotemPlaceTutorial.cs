using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemPlaceTutorial : MonoBehaviour
{
    [SerializeField] float Speed = 1;
    public GameObject door;
    Color doorColor;
    // Start is called before the first frame update
    void Start()
    {
        doorColor = door.GetComponent<MeshRenderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Totem")
        {
            StartCoroutine(TotemFadeOut());
            Debug.Log("Working");
        }
    }

    IEnumerator TotemFadeOut()
    {
        float alphat = door.GetComponent<MeshRenderer>().material.color.a;
        yield return new WaitForSeconds(3f);
        while (alphat > 0)
        {
            alphat -= Time.deltaTime * Speed;
            door.GetComponent<MeshRenderer>().material.color = new Color(doorColor.r, doorColor.g, doorColor.b, alphat);
            yield return null;
        }
        if (alphat <= 0)
        {
            door.SetActive(false);
        }

        yield return null;
    }

}
