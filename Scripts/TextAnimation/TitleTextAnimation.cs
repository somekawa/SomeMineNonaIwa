﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleTextAnimation : MonoBehaviour
{
    public Animator[] textAnim_;
    private int randomVibCnt_;
    public float randomCnt_;
    public float textCnt_;

    // Start is called before the first frame update
    void Start()
    {
        textCnt_ = 0;
        randomVibCnt_ = Random.Range(1, 5);
        randomCnt_ = Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        textCnt_ += Time.deltaTime;
        if(textCnt_>= randomVibCnt_- randomCnt_)
        {
            for (int i = 0; i < textAnim_.Length; i++)
            {
                textAnim_[i].SetBool("vibFlag", true);
            }
        }

        if (textCnt_>= randomVibCnt_)
        {
            textCnt_ = 0;
            for (int i = 0; i < textAnim_.Length; i++)
            {
                textAnim_[i].SetBool("vibFlag", false);
            }
            randomVibCnt_ = Random.Range(1, 5);
            randomCnt_ = Random.value;
        }
    }
}
