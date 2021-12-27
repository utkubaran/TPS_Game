using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [SerializeField]
    Transform gunPosition;

    [SerializeField]
    float gunDamage = 20f;

    [SerializeField]
    float timeBetweenShots = 1.5f;

    private FieldOfView enemyFieldOfView;

    private bool isPlayerInSight, isShot;

    private float timeRemainingForNextShot;

    // Start is called before the first frame update
    void Start()
    {
        enemyFieldOfView = GetComponent<FieldOfView>();
        isPlayerInSight = enemyFieldOfView.IsTargetInSight;
        isShot = false;
        timeRemainingForNextShot = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        isPlayerInSight = enemyFieldOfView.IsTargetInSight;

        if (isPlayerInSight && !isShot)
        {
            isShot = true;
            RaycastHit hit;

            if (Physics.Raycast(gunPosition.position, gunPosition.forward, out hit, enemyFieldOfView.viewRadius))
            {
                hit.transform.GetComponent<IDamageable>()?.GetDamage(gunDamage);
            }
        }
        
        ShootTimer();
    }

    private void ShootTimer()
    {
        timeRemainingForNextShot -= Time.deltaTime;

        if (timeRemainingForNextShot <= 0f)
        {
            isShot = false;
            timeRemainingForNextShot = timeBetweenShots;
        }
    }
}
