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
    public GameObject resultsUI;
    public AudioClip celebrationMusic;
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
        lives=100;
        decorationCount=0;
        resultsUI.SetActive(true);
        AudioManager.Instance.PlayMusicSource2(celebrationMusic,1,true);
        AudioManager.Instance.StopMusicSource1();
        print("WIN");
    }
    public void Lose(){
        lives=3;
        decorationCount=0;
        print("LOSE");
        SceneManager.LoadScene(0);
    }
    public void restart(){
        SceneManager.LoadScene(0);
    }
    public void Quit(){
        Application.Quit();   }
}
