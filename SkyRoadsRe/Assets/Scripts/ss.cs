using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class ss : MonoBehaviour
{
    //Component Variables
    Rigidbody playerRb;
    [SerializeField] PlayerInput input;
    [SerializeField] GameObject cam;
    CinemachineVirtualCamera vcam;

    //Input Variables
    Vector2 move;
    Vector3 movement;
    bool isMovementPressed;
    [SerializeField]float movementSpeed;
    [SerializeField]float acceleration=1f;
    [SerializeField]float accelerationMultiplier=.02f;
    [SerializeField]float maxSpeed;
    [SerializeField]float minSpeed;
    [SerializeField]float jumpForce = 2.0f;
    bool isFalling;
    Vector3 jump;
    

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, .01f, 0.0f);

        input.actions["Move"].performed += OnMovementPressed;
        input.actions["Move"].started += OnMovementPressed;
        input.actions["Move"].canceled += OnMovementPressed;
        

    }

    
    void FixedUpdate()
    {
       
        Jump();

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
          
            playerRb.useGravity = false;
            
            isFalling=false;
            HandleMovement();
            
        }
        else
        {
            playerRb.useGravity = true;
            isFalling=true;
             if(isMovementPressed)
             {   
               
                 playerRb.velocity = new Vector3(-movement.x,movement.y,0) * Time.deltaTime * movementSpeed;
             }
           
            StartCoroutine(Death());
            
        }
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(1.5f);
        if(isFalling)
         playerRb.AddRelativeTorque(1f,0,0f);
        yield return new WaitForSeconds(2f);
        
        if(isFalling){
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
        }
        Debug.Log("Destroyed");
        yield return null;

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

            if(movement.z>0)movement.z=-movement.z;
            acceleration+=accelerationMultiplier;
            if(acceleration>maxSpeed)acceleration=maxSpeed;
   
            playerRb.velocity = new Vector3(-movement.x,movement.y,movement.z*acceleration) * Time.deltaTime * movementSpeed;
        }
        else 
        {
            acceleration-=accelerationMultiplier;
            if(acceleration<minSpeed)acceleration=minSpeed;
            
            playerRb.velocity = new Vector3(0,playerRb.velocity.y,-acceleration) * Time.deltaTime * movementSpeed;
            

        }
    }


    void Jump()
    {
        if(input.actions["Jump"].WasPressedThisFrame())
        {
           playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
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
