using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] float clipCooldown = 1f;
    float timer;

    FirstPersonMovementRB player;

    bool change = false;
    int index = 8;

    // Start is called before the first frame update
    void Start()
    {
        if (FindObjectOfType<FirstPersonMovementRB>(false))
        {
            player = FindObjectOfType<FirstPersonMovementRB>(false);
        }
        else
        {
            Debug.LogError("Failed to grab script 'LoadSceneAsync' from the scene.");
        }

        timer = clipCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.rb.velocity.magnitude > 0.1f && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(player.rb.velocity.magnitude > 0.1f)
        {
            int i = (int)Random.Range(0.1f, 2);
            switch (player.moveState)
            {
                case Move3DStateMachine.MoveState.Ground:
                    Debug.Log("Enter Ground");
                    switch (player.biomeState)
                    {
                        case Move3DStateMachine.BiomeState.Tree:
                            Debug.Log("TreeStep");
                            AudioManager.Play((i > 0 ? "GroundStep(1)" : "GroundStep"), 50f);
                            break;
                        case Move3DStateMachine.BiomeState.Snow:
                            Debug.Log("SnowStep");
                            AudioManager.Play((i > 0 ? "SnowStep(1)" : "SnowStep"), 50f);
                            break;
                        case Move3DStateMachine.BiomeState.Sand:
                            Debug.Log("SandStep");
                            AudioManager.Play((i > 0 ? "SandStep(1)" : "SandStep"), 50f);
                            break;
                    }

                    break;

                case Move3DStateMachine.MoveState.Ice:
                    AudioManager.Play((i > 0 ? "IceStep(1)" : "IceStep"), 50f);
                    break;
            }
            timer = clipCooldown;
        }

        if (change)
        {
            Debug.Log("entered");
            change = false;
            AudioManager.PlayLoop(index, 50);
            index++;
        }
    }

    void ChangeBG()
    {
        change = true;
    }

}

