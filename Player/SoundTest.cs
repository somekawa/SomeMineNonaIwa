﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
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

    // なり終わったらdestroyしないといけない???
    private void Update()
    {
        if(!audioSource.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
