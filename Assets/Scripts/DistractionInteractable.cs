using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractionInteractable : Interactable
{
    public GameObject distractionGO;

    public override void whileInteracting(){
        base.whileInteracting();
    }

    public override void whileNotInteracting(){
        base.whileNotInteracting();
    }

    //Runs in update while being interacted upon
    public override void OnInteract(){
        base.OnInteract();
    }

    public override void OnInteractComplete(){
        base.OnInteractComplete();
        GameObject.Instantiate(distractionGO, this.transform.position, Quaternion.identity);
    }

    public override void UnInteract(){
        base.UnInteract();
    }

}
