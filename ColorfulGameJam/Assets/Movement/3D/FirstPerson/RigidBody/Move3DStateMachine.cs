using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// *
/// * Script by Edgar
/// *
/// * enum class that holds players state of movement
/// *
/// </summary>
public class Move3DStateMachine : MonoBehaviour
{
    public enum MoveState
    {
        Ground,
        Ice,
        Air
    }

    [HideInInspector]
    public MoveState moveState;

    public enum BiomeState
    {
        Tree,
        Snow,
        Sand
    }

    public BiomeState biomeState;

    public void SetMoveState(int i)
    {
        switch (i)
        {
            case 0:
                moveState = MoveState.Ground;
                break;
            case 1:
                moveState = MoveState.Ice;
                break;
            case 2:
                moveState = MoveState.Air;
                break;
        }
    }

    public void SetBiomeState(int i)
    {
        switch (i)
        {
            case 0:
                biomeState = BiomeState.Tree;
                break;
            case 1:
                biomeState = BiomeState.Snow;
                break;
            case 2:
                biomeState = BiomeState.Sand;
                break;
        }

    }
}
