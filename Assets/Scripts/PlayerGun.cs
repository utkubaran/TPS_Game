using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    float timeBetweenShots = 1f;

    [SerializeField]
    LayerMask targetMask;

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

            if (Physics.Raycast(this.transform.position, transform.forward, out hit, playerFieldOfView.viewRadius))
            {
                // todo add shooting events
                Debug.Log(hit.transform.name);
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
