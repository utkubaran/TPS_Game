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

    private Vector3 startPosition;

    private bool isTurned;

    private float distanceCovered = 0f, rotationAngleMultiplier = 1f, timeRemaining, rotationTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        enemyAnimationStateController = GetComponent<EnemyAnimationStateController>();
        startPosition = transform.position;
        timeRemaining = patrolTurnAroundTime;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(transform.rotation);
        Patrol();
    }

    private void Patrol()
    {
        distanceCovered = Vector3.Distance(startPosition, transform.position);
        
        if (distanceCovered <= patrolRange)
        {
            characterController?.Move(this.transform.forward * movementSpeed * Time.deltaTime);
        }
        else
        {
            characterController?.Move(Vector3.zero);
            Rotate();
        }
    }

    private void Rotate()
    {
        if (!isTurned)
        {
            isTurned = true;
            Vector3 rotationVector2 = new Vector3(0f, this.transform.rotation.eulerAngles.y + 180f * rotationAngleMultiplier, 0f);
            Quaternion rotationVector = new Quaternion(0f, rotationAngleMultiplier, 0f, 0f);
            transform.DORotate(rotationVector2, rotationTime);
        }
        else
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0f)
            {
                isTurned = false;
                rotationAngleMultiplier *= -1f;
                timeRemaining = patrolTurnAroundTime;
                startPosition = this.transform.position;
            }
        }
    }
}
