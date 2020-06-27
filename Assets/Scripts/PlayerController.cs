using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float speed;
    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Move(input * speed);
    }

    void Move(Vector3 input){
        //TODO use camera position to make the player move at what looks like the same speed in all directions
        rigid.rotation = Quaternion.LookRotation(input);
        
        input.y = 0;
        rigid.velocity = input;
    }
}
