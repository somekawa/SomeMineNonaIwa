using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSound : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.Log("audioSourceがnull");
        }
    }

    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            // 鳴り終わったらdestroyする
            Destroy(this.gameObject);
        }
    }
}
