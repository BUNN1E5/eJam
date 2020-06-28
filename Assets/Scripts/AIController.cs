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

        Distracted,
    }
    public state myState;
    public LayerMask suspiciousObjects;

    // audio
    public AudioClip[] busyClips;
    public AudioClip susClip;
    public AudioClip shufflingClip;
    public AudioClip[] investigateClip;
    public AudioClip alertClip;
    private bool audioBoolOne = false;
    private bool audioBoolTwo = false;
    GameObject distractionGO;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        agent=GetComponent<NavMeshAgent>();
        waypointIndex=idleWaypoints.Length;
        StartCoroutine(FindNewDestination(waypointTaskTime));
        suspicionProgress=0;
        lineRenderer=GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(myState==state.Busy){
            lineRenderer.enabled=false;
        }
        else{
             lineRenderer.enabled=true;
        }
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,agent.destination);
        if(myState==state.Suspicous){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){               
                StartCoroutine(GrowSuspician());
            }
        } 
        if(myState==state.Distracted){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){
                Destroy(distractionGO,waypointTaskTime);               
                StartCoroutine(FindNewDestination(waypointTaskTime));
            }
        } 
        if(myState==state.Walking){
            if(Vector3.Distance(transform.position,agent.destination)<destinationThreshold){
                StartCoroutine(FindNewDestination(waypointTaskTime));
            }
            CheckForSuspiciousObjects();
        }
    }
    public void DistractAI(GameObject distrcation){
        myState=state.Distracted;
        StopCoroutine(GrowSuspician());
        agent.destination=distrcation.transform.position;
        distractionGO=distrcation;
    }
    IEnumerator GrowSuspician(){
        if (audioBoolTwo != true)
        {
            audioBoolTwo = true;
            AudioManager.Instance.PlaySFX(investigateClip[Random.Range(0, investigateClip.Length)], 0.8f, 1.0f);
            AudioManager.Instance.PlaySFX(shufflingClip, 0.8f, 1.0f);
            StartCoroutine(MakeAudioBoolTwoFalse());
        }
        myState=state.Investigating;
        yield return new WaitForSeconds(suspicionSpeed);
        suspicionProgress++;
        Destroy(subjectOfSuspician);
        myState=state.Walking;
    }
    IEnumerator FindNewDestination(float waitTime){
        myState=state.Busy;
        // play im busy
        AudioManager.Instance.PlaySFX(busyClips[Random.Range(0, busyClips.Length)], 0.8f, 1.0f);
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
                        if (audioBoolOne != true)
                        {
                            audioBoolOne = true;
                            AudioManager.Instance.PlaySFX(susClip, 0.8f, 1.0f);
                            AudioManager.Instance.PlaySFX(alertClip, 0.6f, 1.0f);
                            StartCoroutine(MakeAudioBoolOneFalse());
                        }
                        myState =state.Suspicous;
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
    IEnumerator MakeAudioBoolOneFalse()
    {
        yield return new WaitForSecondsRealtime(7f);
        audioBoolOne = false;
    }
    IEnumerator MakeAudioBoolTwoFalse()
    {
        yield return new WaitForSecondsRealtime(4f);
        audioBoolTwo = false;
    }
}
