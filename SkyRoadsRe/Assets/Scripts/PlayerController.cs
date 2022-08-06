using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
public class PlayerController : MonoBehaviour
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
    [SerializeField]float jumpChecker = .2f;
    bool isFalling;
    bool jumpPressed;
    bool isJumping;
    bool anotherJumpVariable;
    Vector3 jump;
    int start=0;
    
    //Particle Component
    [SerializeField]GameObject flame1;
    [SerializeField]GameObject flame2;
    ParticleSystem particleFlame1;
    ParticleSystem particleFlame2;

    [SerializeField] GameObject blast;
    ParticleSystem blastParticle;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, .01f, 0.0f);
        particleFlame1=flame1.GetComponent<ParticleSystem>();
        particleFlame2=flame2.GetComponent<ParticleSystem>();
        blastParticle=blast.GetComponent<ParticleSystem>();
        var main=particleFlame1.main;
        var main1=particleFlame2.main;

        main.startLifetime =0f;
        main1.startLifetime =0f;


    }

    private void Awake() 
    {
        input.actions["Move"].performed += OnMovementPressed;
        input.actions["Move"].started += OnMovementPressed;
        input.actions["Move"].canceled += OnMovementPressed;
        input.actions["Jump"].started+=OnJumpPressed;
        input.actions["Jump"].canceled+=OnJumpPressed;

  
    }

    
    void FixedUpdate()
    {  
        HandleGravity();

        if(movement.z!=0)
        {
            particleFlame1.Play();
            particleFlame2.Play();
            start=1;
         }

        if(start==1)
        {
        HandleMovement();  
        
        Jump();
        }
        

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
      
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit1 ,jumpChecker))
        {   
            Debug.Log(isJumping);
            isJumping=false;
            anotherJumpVariable=false;
          
         
        }
       
        if(Physics.Raycast(transform.position,Vector3.down,out RaycastHit hit ,20.0f))
        {
            isFalling=false;
            

        }

        else
        {
          
           isFalling=true;
           if(isMovementPressed) playerRb.MovePosition(transform.position+new Vector3(-movement.x,movement.y,0) * Time.deltaTime * movementSpeed);
       
         if(playerRb.velocity.y < -10)
            {
            Death();

            }   
            
        }
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    /// <summary>
    /// This function handle death of the rocket
    /// </summary>
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Death()
    {
        
        
        var temp = Instantiate(blastParticle,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
        temp.Play();
        
        
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
        var main=particleFlame1.main;
        var main1=particleFlame2.main;
        if(isMovementPressed)
        {   
            if(movement.z!=0)
            {
  
                acceleration+=accelerationMultiplier;
                main.startLifetime =.69f;
                main1.startLifetime =.69f;
            }

            if(acceleration>maxSpeed)acceleration=maxSpeed;
            if(acceleration<minSpeed)acceleration=minSpeed;  
           
            playerRb.MovePosition(transform.position+new Vector3(-movement.x,movement.y,-acceleration) * Time.deltaTime * movementSpeed);

        }

        else 
        {
           
            acceleration-=accelerationMultiplier;
            main.startLifetime =.4f;
            main1.startLifetime =.4f;

            if(acceleration<minSpeed)acceleration=minSpeed; 

            playerRb.MovePosition(transform.position+new Vector3(0,0,-acceleration) * Time.deltaTime * movementSpeed);
        }

    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 
    /// <summary>
    /// This function handle jump movement of the rocket
    /// </summary>
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void Jump()
    {
        if(jumpPressed&&!isJumping)
        {
          
            isJumping=true;
            anotherJumpVariable=true;
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


    void OnJumpPressed(InputAction.CallbackContext ctx)
    {
        jumpPressed=!jumpPressed;
    }
}
