using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIController : MonoBehaviour
{
    NavMeshAgent agent;
    public NavMeshSurface surface;
    public float destinationThreshold;
    public LayerMask navLayer;
    public float walkLength;
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        agent.SetDestination(GetRandomPosition());
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){
            DestinationReached();
        }
    }
    void DestinationReached(){
        agent.SetDestination(GetRandomPosition());
    }
    Vector3 GetRandomPosition(){
                                  
        
        Vector3 randomDirection=Random.onUnitSphere;
        randomDirection.y=0;
        randomDirection.Normalize();
        RaycastHit hit;
        if(Physics.Raycast(transform.position+Vector3.up,randomDirection,out hit,100,navLayer)){
            NavMeshHit navHit;
            NavMesh.SamplePosition (hit.point, out navHit, walkLength,navLayer); 
            return navHit.position;
        }
        return agent.destination;

    }
}
