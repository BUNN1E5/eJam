using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AIController>().DistractAI(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
