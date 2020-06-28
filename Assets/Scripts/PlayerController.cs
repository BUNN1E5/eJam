using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    public float walkSpeed = 1f;
    public float sprintModifier = 1.25f;
    
    public bool isSprinting = false;

    public float SphereCastRadius;
    public float maxInteractDistance;

    public Interactable currentInteraction;

    public GameObject heldObject;
    public Transform heldPosition;

    Rigidbody rigid;

    Animator animator;

    public GameObject distractionPrefab;

    public GameObject decorationPrefab;
    bool canPlaceDecoration = true;
    float cooldownTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        isSprinting = Input.GetButton("Sprint");

        if(isSprinting){
            Move(input, walkSpeed * sprintModifier);
        } else{
            Move(input, walkSpeed);
        }

        if(Input.GetButtonDown("Action0")){
            if(canPlaceDecoration){
                PlaceDecoration();
                StartCoroutine(cooldown(cooldownTime));
            }
            //Interact();
            
        } else if(Input.GetButtonUp("Action0")){
            //UnInteract();
        }
        if(Input.GetButtonDown("Action1")){
            if(!(FindObjectOfType<AIController>().myState==state.Distracted))
                PlaceDistraction();
            //Interact();
            
        }

        animator.SetFloat("Speed", rigid.velocity.magnitude);
    }

    IEnumerator cooldown(float time){
        canPlaceDecoration = false;
        yield return new WaitForSeconds(time);
        canPlaceDecoration = true;
    }

    public void PlaceDecoration(){
        GameManager.decorationCount++;
        Instantiate(decorationPrefab,transform.position,transform.rotation);
    }

    public void PlaceDistraction(){
        Instantiate(distractionPrefab,transform.position,transform.rotation);
    }

    Interactable findInteractable(){
        RaycastHit hit;
        Physics.SphereCast(this.transform.position, SphereCastRadius, this.transform.forward, out hit, maxInteractDistance, ~LayerMask.GetMask("Player"));

        Debug.DrawLine(this.transform.position, hit.point);
        Debug.DrawRay(this.transform.position, this.transform.forward);

        if(hit.collider != null){
            if(hit.collider.tag.Equals("Interactable")){
                return hit.transform.gameObject.GetComponent<Interactable>();
            }
        }
        return null;
    }

    void Interact(){
        currentInteraction = findInteractable();
        currentInteraction.OnInteract();
    }

    void UnInteract(){
        currentInteraction.UnInteract();
        currentInteraction = null;
    }

    void Move(Vector3 input, float speed){
        //TODO use camera position to make the player move at what looks like the same speed in all direction       
        input.Normalize();

        input.y = 0;
        rigid.velocity = input * speed;
        
        if(rigid.velocity.magnitude > .1f) //So we maintain our last moved
            rigid.rotation = Quaternion.LookRotation(rigid.velocity);
    }
}
