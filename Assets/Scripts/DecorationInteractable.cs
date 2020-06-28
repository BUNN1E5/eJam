using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationInteractable : Interactable{


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
    }

    public override void UnInteract(){
        base.UnInteract();
    }

}
