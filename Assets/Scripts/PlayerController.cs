using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 1f;
    public float sprintModifier = 1.25f;
    
    public bool isSprinting = false;

    Rigidbody rigid;
    ParticleSystem sprintClouds;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        isSprinting = Input.GetButton("Sprint");

        if(isSprinting){
            sprintClouds.Play();
            Move(input * walkSpeed * sprintModifier);
        } else{
            sprintClouds.Pause();
            Move(input * walkSpeed);
        }
    }

    void Move(Vector3 input){
        //TODO use camera position to make the player move at what looks like the same speed in all directions
        rigid.rotation = Quaternion.LookRotation(input);
        
        input.y = 0;
        rigid.velocity = input;
    }
}
