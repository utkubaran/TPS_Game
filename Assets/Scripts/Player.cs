using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    float playerHealth = 100f;

    private PlayerAnimationStateController playerAnimationStateController;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimationStateController = GetComponent<PlayerAnimationStateController>();
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
        if (playerHealth <= 0f)
        {
            Debug.Log("Player is dead!");
            playerAnimationStateController.currentState = PlayerAnimationStateController.PlayerState.Dead;
            Destroy(this.gameObject, 2f);
        }
    }
}
