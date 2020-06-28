using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int requiredDecorationsForWin;
    public static int decorationCount=0;
    public static int lives=3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(decorationCount==requiredDecorationsForWin){
            Win();
        }
    }
    public void Win(){
        print("WIN");
    }
}
