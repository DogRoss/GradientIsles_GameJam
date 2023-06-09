using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Need the platform and the player objects.")]
    public GameObject platform;
    public GameObject player;
    public ScreenShake playerShake;

    [Header("Where you want the platform to be. P.S. tag the Player with 'Player'.")]
    public Transform platformMovePosition;
    bool onPlatform = false;

    SphereCollider Sc;

    [SerializeField] float platformSpeed;

    void Start()
    {
        Sc = GetComponent<SphereCollider>();
    }

    public bool done = false;
    // Update is called once per frame
    void Update()
    {
        if (onPlatform)
        {
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, platformMovePosition.position, platformSpeed * Time.deltaTime);
            playerShake.ShakeScreen(120f);
        }

        if (platform.transform.position == platformMovePosition.position && !done)
        {
            onPlatform = false;
            playerShake.ShakeScreen(-1f);
            platform.transform.DetachChildren();
            done = true;
        }
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Sc.enabled = false;
            player.transform.SetParent(platform.transform);
            onPlatform = true;
        }
    }
}
