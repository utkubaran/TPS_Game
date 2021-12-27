using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    float enemyHealth;

    private EnemyAnimationStateController enemyAnimationStateController;

    private float deathSequenceTimer = 1.5f;

    private bool isDead;

    void Start() 
    {
        enemyAnimationStateController = GetComponent<EnemyAnimationStateController>();
        isDead = false;
    }

    void Update()
    {
        Die();
    }

    public void GetDamage(float damage)
    {
        enemyHealth -= damage;
    }

    public void Die()
    {
        if (enemyHealth <= 0f && !isDead)
        {
            isDead = true;
            enemyAnimationStateController.currentState = EnemyAnimationStateController.EnemyState.Dead;
            enemyAnimationStateController?.ChangeAnimationState();
            GetComponent<EnemyMovementController>().enabled = false;
            // GetComponent<CharacterController>()?.Move(Vector3.zero);        // todo refactor: plays death animation multiple times
            StartCoroutine(DeathSequence());
        }
    }

    private IEnumerator DeathSequence()
    {
        yield return new WaitForSeconds(deathSequenceTimer);
        this.gameObject.SetActive(false);
    }
}
