using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    float enemyHealth;

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
        if (enemyHealth <= 0f)
        {
            Destroy(this.gameObject, 1f);
        }
    }
}
