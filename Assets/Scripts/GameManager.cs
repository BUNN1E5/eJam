using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public int requiredDecorationsForWin;
    public static int decorationCount=0;
    public static int lives=3;
    public bool over=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(decorationCount==requiredDecorationsForWin&&!over){
            Win();
        }
        if(lives==0&&!over){
            Lose();
        }
    }
    public void Win(){
        print("WIN");
    }
    public void Lose(){
        print("LOSE");
        SceneManager.LoadScene(0);
    }
}
