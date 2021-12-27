using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationStateController : MonoBehaviour
{
    [HideInInspector]
    public enum PlayerState { Idle, Walking, Shooting, Dead}

    [HideInInspector]
    public PlayerState currentState;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = PlayerState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        // CheckForAnimationState();
    }

    /*
    private void CheckForAnimationState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", false);
                break;
            case PlayerState.Walking:
                animator?.SetBool("isWalking", true);
                animator?.SetBool("isShooting", false);
                break;
            case PlayerState.Shooting:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", true);
                break;
            case PlayerState.Dead:
                animator?.SetTrigger("isDead");
                break;
            default:
                Debug.Log("NO STATE!");
                break;
        }
    }
    */

    public void ChangeAnimationState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", false);
                break;
            case PlayerState.Walking:
                animator?.SetBool("isWalking", true);
                animator?.SetBool("isShooting", false);
                break;
            case PlayerState.Shooting:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", true);
                break;
            case PlayerState.Dead:
                animator?.SetTrigger("isDead");
                break;
            default:
                Debug.Log("NO STATE!");
                break;
        }
    }
}
