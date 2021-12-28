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

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    float impactForce;

    private FieldOfView playerFieldOfView;

    private Transform targetEnemy;

    private bool isEnemyInSight, isShot;

    private float timeRemainingForNextShot = 0f;
    
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
                GameObject impactGO =  Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal)) as GameObject;
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(hit.normal * impactForce);
                }
                Destroy(impactGO, 0.5f);
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
