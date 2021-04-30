using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseControl : MonoBehaviour
{
    public float parameter_ = 0.0f; // パラメーター(0.0f～1.0f)

    private RawImage rawImageN_;
    public float maxN_;             // ノイズの最大値

    private RawImage rawImageB_;
    public float maxB_;             // 血の最大値
    private float minB_ = 0.0f;     // 血の最小値
    private float startB_ = 0.0f;   // 血出現開始時間

    private RawImage rawImageSN_;   // 横ノイズ
    private float startSN_;
    public float moveTimeSN_;       // 稼働時間

    // Start is called before the first frame update
    void Start()
    {
        RawImage[] rawImageList =gameObject.GetComponentsInChildren<RawImage>();
        foreach(RawImage rawImage in rawImageList)
        {
            if(rawImage.name== "N_RawImage")
            {
                rawImageN_ = rawImage;
                rawImageN_.material.SetFloat("alpha",0.0f); 
            }
            else if(rawImage.name == "B_RawImage")
            {
                rawImageB_ = rawImage;
                rawImageB_.material.SetFloat("alpha", 0.0f);
            }
            else if (rawImage.name == "SN_RawImage")
            {
                rawImageSN_ = rawImage;
                rawImageSN_.material.SetFloat("flag", 0.0f);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (parameter_ < 1.0f)
        {
            rawImageN_.material.SetFloat("alpha", parameter_ * maxN_ );
        }
        rawImageN_.material.SetFloat("time", Time.time);

        // 血(80以上から出現)
        if (parameter_ >= 0.8f)
        {
            if (startB_ == 0.0f) 
            {
                startB_ = Time.time;
            }
            if ((minB_ == 0.0f) && (rawImageB_.material.GetFloat("alpha") >= 0.6f)) 
            {
                // 最小値の設定
                minB_ = 0.6f;
            }

            float alpha = (Mathf.Abs(Mathf.Sin(Time.time - startB_)) * maxB_);
            if (alpha <= minB_) 
            {
                alpha = minB_;
            }
            rawImageB_.material.SetFloat("alpha", alpha);
        }
        else
        {    
            if (Mathf.Floor(rawImageB_.material.GetFloat("alpha") * 100) / 100 > 0.0f) 
            {
                float alpha = (Mathf.Abs(Mathf.Sin(Time.time - startB_)) * maxB_);
                rawImageB_.material.SetFloat("alpha", alpha);
            }
            else
            {
                startB_ = 0.0f;
                minB_ = 0.0f;
                rawImageB_.material.SetFloat("alpha", 0.0f);
            }
        }
        //Debug.Log(rawImageB_.material.GetFloat("alpha"));

        if(rawImageSN_.material.GetFloat("flag")!=0.0f)
        {
            if(startSN_==0.0f)
            {
                startSN_ = Time.time;
            }
            rawImageSN_.material.SetFloat("time", Time.time);
            if (moveTimeSN_ < Time.time - startSN_) 
            {
                rawImageSN_.material.SetFloat("flag", 0.0f);
                startSN_ = 0.0f;
            }
        }
    }

    public void DiscoveryNoise()
    {
        rawImageSN_.material.SetFloat("flag", 1.0f);
        startSN_ = Time.time;
    }
}
