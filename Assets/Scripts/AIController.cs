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
    public int suspicionProgress;
    public Transform[] idleWaypoints;
    int waypointIndex;
    public float waypointTaskTime;
    public float suspicionSpeed;
    public float visionRadius;
    [Range(0,1)]
    public float fovRange;

    GameObject subjectOfSuspician;
    [System.Serializable]
    public enum state{
        Busy,
        Walking,
        Suspicous,

        Investigating,
    }
    public state myState;
    public LayerMask suspiciousObjects;
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        waypointIndex=idleWaypoints.Length;
        StartCoroutine(FindNewDestination(waypointTaskTime));
        suspicionProgress=0;
    }

    // Update is called once per frame
    void Update()
    {   
        if(myState==state.Suspicous){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){               
                StartCoroutine(GrowSuspician());
            }
        } 
        if(myState==state.Walking){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){
                StartCoroutine(FindNewDestination(waypointTaskTime));
            }
            CheckForSuspiciousObjects();
        }
    }
    IEnumerator GrowSuspician(){
        myState=state.Investigating;
        yield return new WaitForSeconds(suspicionSpeed);
        suspicionProgress++;
        Destroy(subjectOfSuspician);
        myState=state.Walking;
    }
    IEnumerator FindNewDestination(float waitTime){
        myState=state.Busy;
        agent.isStopped=true;
        yield return new WaitForSeconds(waitTime);
        myState=state.Walking;     
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
        Collider[] hits=Physics.OverlapSphere(transform.position,visionRadius,suspiciousObjects);
        foreach(Collider hit in hits){
            Vector3 toTarget = (hit.transform.position - transform.position).normalized;
             
            if (Vector3.Dot(toTarget, transform.forward) > fovRange) {
                RaycastHit los;
                Debug.DrawLine(hit.transform.position,transform.position);
                if(Physics.Raycast(transform.position,hit.transform.position-transform.position,out los,visionRadius)){
                    if(los.collider.CompareTag("Sus")){
                        myState=state.Suspicous;
                        NavMeshHit navHit;
                        NavMesh.SamplePosition(los.point, out navHit, 1,navLayer); 
                        agent.SetDestination(navHit.position); 
                        subjectOfSuspician=los.collider.gameObject;
                    }
                }
            }
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
