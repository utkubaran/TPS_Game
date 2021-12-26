using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationStateController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [HideInInspector]
    public enum PlayerState { Idle, Walking, Shooting, Dead}

    [HideInInspector]
    public PlayerState currentState;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        CheckForAnimationState();
    }

    private void CheckForAnimationState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                Debug.Log("I'm idle!");
                break;
            case PlayerState.Walking:
                Debug.Log("I'm walking");
                break;
            case PlayerState.Shooting:
                Debug.Log("I'm shooting!");
                break;
            case PlayerState.Dead:
                Debug.Log("I'm dead!");
                break;
            default:
                Debug.Log("NO STATE!");
                break;
        }
    }
}
