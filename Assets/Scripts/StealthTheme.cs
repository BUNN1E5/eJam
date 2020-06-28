using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthTheme : MonoBehaviour
{
    public AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayStealthTheme());
    }

    IEnumerator PlayStealthTheme()
    {
        while (true)
        {
            AudioManager.Instance.PlayMusicSource1(audioClips[Random.Range(0, audioClips.Length)], 1.0f, false);
            yield return new WaitForSecondsRealtime(audioClips[0].length);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
