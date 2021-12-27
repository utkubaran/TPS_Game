using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationStateController : MonoBehaviour
{
    public enum EnemyState { Idle, Walking, Shooting, Dead}

    [HideInInspector]
    public EnemyState currentState;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = EnemyState.Idle;
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
            case EnemyState.Idle:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", false);
                break;
            case EnemyState.Walking:
                animator?.SetBool("isWalking", true);
                animator?.SetBool("isShooting", false);
                break;
            case EnemyState.Shooting:
                animator?.SetBool("isWalking", false);
                animator?.SetBool("isShooting", true);
                break;
            case EnemyState.Dead:
                animator?.SetTrigger("isDead");
                break;
            default:
                Debug.Log("NO STATE!");
                break;
        }
    }
}
