using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float textSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, textSpeed * Time.deltaTime, 0);
    }
}
