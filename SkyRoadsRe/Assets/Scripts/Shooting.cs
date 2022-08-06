using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Shooting : MonoBehaviour
{
    [SerializeField]PlayerInput input;
    [SerializeField]GameObject bulletHolder;
    [SerializeField]GameObject bullet;
    [SerializeField]float speed;
    // Start is called before the first frame update
    void Start()
    {
        input=GetComponent<PlayerInput>();
        input.actions["Shoot"].started+=OnShooting;
        input.actions["Shoot"].performed+=OnShooting;
        
    }

    void Shoot()
    {
        var temp=Instantiate(bullet,bulletHolder.transform.position,Quaternion.identity);
        //temp.GetComponent<Rigidbody>().AddForce(new Vector3(0,0,-1f)*speed*Time.deltaTime);
        temp.GetComponent<Rigidbody>().velocity=new Vector3(0,0,-1f)*speed*Time.deltaTime;

    }

    void OnShooting(InputAction.CallbackContext ctx)
    {
        Shoot();
    }
}
