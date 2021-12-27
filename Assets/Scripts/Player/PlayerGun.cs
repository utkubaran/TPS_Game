using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    Transform gunPosition;

    [SerializeField]
    float gunDamage = 20f;

    [SerializeField]
    float timeBetweenShots = 1f;

    [SerializeField]
    ParticleSystem muzzleFlash;

    private FieldOfView playerFieldOfView;

    private Transform targetEnemy;

    private bool isEnemyInSight, isShot;

    private float timeRemainingForNextShot;
    
    // Start is called before the first frame update
    void Start()
    {
        playerFieldOfView = GetComponent<FieldOfView>();
        isEnemyInSight = playerFieldOfView.IsTargetInSight;
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
        isEnemyInSight = playerFieldOfView.IsTargetInSight;

        if (isEnemyInSight && !isShot)
        {
            isShot = true;
            RaycastHit hit;
            muzzleFlash?.Play();

            if (Physics.Raycast(gunPosition.position, gunPosition.forward, out hit, playerFieldOfView.viewRadius))
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
