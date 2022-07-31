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
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        input.actions["Move"].performed += OnMovementPressed;
        input.actions["Move"].started += OnMovementPressed;
        input.actions["Move"].canceled += OnMovementPressed;

    }

    
    void Update()
    {
        if(isMovementPressed)
        {
            Debug.Log("Working");
        }
    }
    
    void OnMovementPressed(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
        isMovementPressed = move.x != 0 || move.y != 0;
        movement.x = move.x;
        movement.z = move.y;
    }
}
