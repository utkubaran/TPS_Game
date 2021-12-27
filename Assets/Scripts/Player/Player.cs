using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    float playerHealth = 100f;

    private PlayerAnimationStateController playerAnimationStateController;

    private bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationStateController = GetComponent<PlayerAnimationStateController>();
        isDead = false;
    }

    void Update()
    {
        Die();
    }

    public void GetDamage(float damage)
    {
        playerHealth -= damage;
    }

    public void Die()
    {
        if (playerHealth <= 0f && !isDead)
        {
            Debug.Log("Player is dead!");
            playerAnimationStateController.currentState = PlayerAnimationStateController.PlayerState.Dead;
            playerAnimationStateController.ChangeAnimationState();
            GetComponent<PlayerMovementController>().enabled = false;
            EventManager.OnLevelFail?.Invoke();
        }
    }
}
