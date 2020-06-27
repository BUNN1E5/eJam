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
    [Range(0,1)]
    public float suspicion;
    public Transform[] idleWaypoints;
    int waypointIndex;
    public float waypointTaskTime;
    public float visionRadius;
    [System.Serializable]
    public enum state{
        Busy,
        Idle,
        Suspicous,

    }
    public state myState;
    public LayerMask suspiciousObjects;
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        waypointIndex=idleWaypoints.Length;
        StartCoroutine(FindNewDestination(waypointTaskTime));
        suspicion=0;
    }

    // Update is called once per frame
    void Update()
    {   
        
        
        if(myState==state.Idle){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){
                StartCoroutine(FindNewDestination(waypointTaskTime));
            }
            CheckForSuspiciousObjects();
        }
    }
    IEnumerator FindNewDestination(float waitTime){
        myState=state.Busy;
        agent.isStopped=true;
        yield return new WaitForSeconds(waitTime);
        myState=state.Idle;     
        agent.isStopped=false;
        if(waypointIndex>=idleWaypoints.Length-1){
            waypointIndex=0;
        }
        else{
            waypointIndex++;
        }
        Vector3 nextWaypoint=idleWaypoints[waypointIndex].position;
        NavMeshHit navHit;
        NavMesh.SamplePosition (nextWaypoint, out navHit, 1,navLayer); 
        agent.SetDestination(navHit.position); 
    }
    void CheckForSuspiciousObjects(){
        RaycastHit[] hits=Physics.SphereCastAll(transform.position,visionRadius,Vector3.zero,0,suspiciousObjects);
        foreach(RaycastHit hit in hits){
            suspicion=1;
        }
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
