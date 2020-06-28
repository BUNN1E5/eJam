using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour{

    public float waitTime;
    public float progress = 0;

    public bool isInteracting = false;

    public void Update(){
        if(isInteracting){
            progress += Time.deltaTime;
            whileInteracting();
        } else{
            progress -= Time.deltaTime;
            whileNotInteracting();
        }

        if(progress > waitTime){
            OnInteractComplete();
        }
    }

    public virtual void whileInteracting(){}

    public virtual void whileNotInteracting(){}

    //Runs in update while being interacted upon
    public virtual void OnInteract(){
        isInteracting = true;
    }

    public virtual void OnInteractComplete(){
        isInteracting = false;
    }

    public virtual void UnInteract(){
        isInteracting = true;
    }
}
