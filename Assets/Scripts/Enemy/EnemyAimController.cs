using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimController : MonoBehaviour
{
    private FieldOfView enemyFieldOfView;

    private EnemyAnimationStateController enemyAnimationStateController;

    private bool isPlayerInsight;

    private Transform targetPlayer;

    void Start()
    {
        enemyFieldOfView = GetComponent<FieldOfView>();
        enemyAnimationStateController = GetComponent<EnemyAnimationStateController>();
        isPlayerInsight = enemyFieldOfView.IsTargetInSight;
    }

    void Update()
    {
        LookAtPlayer();
    }

    private void LookAtPlayer()
    {
        isPlayerInsight = enemyFieldOfView.IsTargetInSight;
        
        if (!isPlayerInsight) return;

        targetPlayer = enemyFieldOfView.Target;
        Vector3 directionToPlayer = targetPlayer.position - this.transform.position;
        transform.rotation = Quaternion.LookRotation(directionToPlayer);
        enemyAnimationStateController.currentState = EnemyAnimationStateController.EnemyState.Shooting;
        enemyAnimationStateController.ChangeAnimationState();
    }
}
