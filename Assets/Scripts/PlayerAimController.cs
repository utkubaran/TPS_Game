using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    private FieldOfView playerFieldOfView;
    private PlayerAnimationStateController playerAnimationStateController;

    private Transform targetEnemy;

    private bool isEnemyInSight;

    // Start is called before the first frame update
    void Start()
    {
        playerFieldOfView = GetComponent<FieldOfView>();
        playerAnimationStateController = GetComponent<PlayerAnimationStateController>();
        isEnemyInSight = playerFieldOfView.IsTargetInSight;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtEnemy();
    }

    private void LookAtEnemy()
    {
        isEnemyInSight = playerFieldOfView.IsTargetInSight;

        if (!isEnemyInSight) return;

        targetEnemy = playerFieldOfView.Target;
        Vector3 directionToEnemy = targetEnemy.position - this.transform.position;
        transform.rotation = Quaternion.LookRotation(directionToEnemy);
        playerAnimationStateController.currentState = PlayerAnimationStateController.PlayerState.Shooting;
    }
}
