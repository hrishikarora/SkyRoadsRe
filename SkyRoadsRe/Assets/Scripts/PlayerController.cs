using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //Component Variables
    Rigidbody playerRb;
    [SerializeField] PlayerInput input;

    //Input Variables
    Vector2 move;
    Vector3 movement;
    bool isMovementPressed;
    [SerializeField]float movementSpeed;
    float acceleration;
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        input.actions["Move"].performed += OnMovementPressed;
        input.actions["Move"].started += OnMovementPressed;
        input.actions["Move"].canceled += OnMovementPressed;

    }

    
    void FixedUpdate()
    {
        HandleGravity();
        HandleMovement();

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    /// <summary>
    /// This function handle gravity of the rocket
    /// </summary>
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void HandleGravity()
    {
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit ,20.0f))
        {
            Debug.Log("jkdhf");
            playerRb.useGravity = false;
            
        }
        else
        {
            playerRb.useGravity = true;
            playerRb.AddRelativeTorque(1f,0,0f);
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    /// <summary>
    /// This function handle movement of the rocket
    /// </summary>
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void HandleMovement()
    {
        if(isMovementPressed)
        {
            
            Debug.Log(movement);
            playerRb.velocity = new Vector3(-movement.x,movement.y,0) * Time.deltaTime * movementSpeed;
        }
        else playerRb.velocity = new Vector3(0,playerRb.velocity.y,0);
    }



    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    /// <summary>
    /// This function records input from the user's device
    /// </summary>
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void OnMovementPressed(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        isMovementPressed = move.x != 0 || move.y != 0;
        movement.x = move.x;
        movement.z = move.y;
    }
}
