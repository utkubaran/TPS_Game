using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovementController : MonoBehaviour
{
    [SerializeField, Tooltip("Enemy patrols in own Z axis!")]
    float movementSpeed = 3f;

    [SerializeField]
    float patrolRange = 5f;

    [SerializeField, Range(2f ,5f)]
    float patrolTurnAroundTime = 2F;

    private CharacterController characterController;

    private EnemyAnimationStateController enemyAnimationStateController;

    private FieldOfView enemyFieldOfView;

    private Vector3 startPosition;

    private Transform targetPlayer;

    private bool isTurned, isPlayerInSight;

    private float distanceCovered = 0f, rotationAngle = 180f, timeRemaining, rotationTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyAnimationStateController = GetComponent<EnemyAnimationStateController>();
        enemyFieldOfView = GetComponent<FieldOfView>();
        startPosition = transform.position;
        timeRemaining = patrolTurnAroundTime;
        enemyAnimationStateController.currentState = EnemyAnimationStateController.EnemyState.Idle;
        enemyAnimationStateController.ChangeAnimationState();
        isPlayerInSight = enemyFieldOfView.IsTargetInSight;
    }

    void Update()
    {
        Patrol();
    }

    void OnDisable() 
    {
        DOTween.Clear();
    }

    private void Patrol()
    {
        isPlayerInSight = enemyFieldOfView.IsTargetInSight;

        if (isPlayerInSight) return;

        distanceCovered = Vector3.Distance(startPosition, transform.position);
        
        if (distanceCovered <= patrolRange)
        {
            characterController?.Move(this.transform.forward * movementSpeed * Time.deltaTime);
            enemyAnimationStateController.currentState = EnemyAnimationStateController.EnemyState.Walking;
            enemyAnimationStateController.ChangeAnimationState();
        }
        else
        {
            characterController?.Move(Vector3.zero);
            enemyAnimationStateController.currentState = EnemyAnimationStateController.EnemyState.Idle;
            enemyAnimationStateController.ChangeAnimationState();
            Rotate();
        }
    }

    private void Rotate()
    {
        if (!isTurned)
        {
            isTurned = true;
            Vector3 rotationVector = new Vector3(0f, this.transform.rotation.eulerAngles.y + rotationAngle, 0f);
            transform.DORotate(rotationVector, rotationTime);
        }
        else
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                isTurned = false;
                rotationAngle *= -1f;
                timeRemaining = patrolTurnAroundTime;
                startPosition = this.transform.position;
            }
        }
    }
}
