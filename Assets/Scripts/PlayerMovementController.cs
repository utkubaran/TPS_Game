using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementController : MonoBehaviour
{
    [Space, SerializeField, Tooltip("Movement Speed of the Player")]
    float movementSpeed = 5f;

    [SerializeField]
    float rotationSpeed = 100f;

    private CharacterController characterController;

    private FieldOfView playerFieldOfView;
    
    private Vector3 movementDirection;
    
    private float horizontal, vertical;

    private bool isEnemyInSight;


    // Start is called before the first frame update
    void Start()
    {
        // todo add field of view properties
        characterController = GetComponent<CharacterController>();
        playerFieldOfView = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        isEnemyInSight = playerFieldOfView.IsTargetInSight;
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (movementDirection.magnitude >= 0.1)
        {
            characterController.Move(movementDirection * movementSpeed * Time.deltaTime);
            Rotate();
        }
    }

    private void Rotate()
    {
        if (isEnemyInSight) return;

        Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(this.transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
