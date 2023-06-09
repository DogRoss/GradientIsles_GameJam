using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isTotemPlaced = false;
    [Header("determine where to put island and how fast it will go")]
    public Transform islandPosition;
    [SerializeField] float Speed = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTotemPlaced)
        {
            transform.position = Vector3.MoveTowards(transform.position, islandPosition.position, Speed);
        }
    }
}
