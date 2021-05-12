using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSound : MonoBehaviour
{
    private AudioSource audioSource_;   // SE情報を取得して際に保存する

    void Start()
    {
        //Componentを取得
        audioSource_ = GetComponent<AudioSource>();
        if(audioSource_ == null)
        {
            Debug.Log("audioSourceがnull");
        }
    }

    private void Update()
    {
        if(!audioSource_.isPlaying)
        {
            // 鳴り終わったらdestroyする
            Destroy(this.gameObject);
        }
    }
}
