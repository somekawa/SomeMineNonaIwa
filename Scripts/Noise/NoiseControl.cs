using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseControl : MonoBehaviour
{
    public float parameter_         = 0.0f;     // パラメーター(0.0f～1.0f)

    private RawImage rawImageN_;                // ノイズ
    public float maxN_;                         // ノイズの最大値

    private RawImage rawImageB_;                // 血
    public float  maxB_;                        // 血の最大値
    private float startB_           = 0.0f;     // 血出現開始時間
    private const float parameterB_ = 0.8f;     // 血出現開始条件(パラメーター)

    private RawImage rawImageSN_;               // 横ノイズ
    private float startSN_          = 0.0f;     // 横ノイズ開始時間
    public float moveTimeSN_;                   // 稼働時間

    // Start is called before the first frame update
    void Start()
    {
        RawImage[] rawImageList =gameObject.GetComponentsInChildren<RawImage>();
        foreach(RawImage rawImage in rawImageList)
        {
            if(rawImage.name== "N_RawImage")
            {
                // ノイズ
                rawImageN_ = rawImage;
                rawImageN_.material.SetFloat("alpha",0.0f); 
            }
            else if(rawImage.name == "B_RawImage")
            {
                // 血
                rawImageB_ = rawImage;
                rawImageB_.material.SetFloat("alpha", 0.0f);
            }
            else if (rawImage.name == "SN_RawImage")
            {
                // 横ノイズ
                rawImageSN_ = rawImage;
                rawImageSN_.material.SetFloat("flag", 0.0f);
            }
            else
            {
                // 何もしない
            }
        }

        //minB_ = maxB_ - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // ノイズ
        if (parameter_ <= 1.0f)
        {
            rawImageN_.material.SetFloat("alpha", parameter_ * maxN_ );
        }
        rawImageN_.material.SetFloat("time", Time.time);

        // 血(0.8以上から出現)
        BloodUpdate();

        // 横ノイズ
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

    private void BloodUpdate()
    {
        float min = 0.0f;
        if (parameter_ < 0.8f)
        {
            if (Mathf.Floor(rawImageB_.material.GetFloat("alpha") * 100.0f) / 100.0f <= 0.0f)
            {
                startB_ = 0.0f;
                rawImageB_.material.SetFloat("alpha", 0.0f);
                return;
            }
        }
        else
        {
            if (startB_ == 0.0f)
            {
                startB_ = Time.time;
            }
            
            //if (Mathf.Ceil(rawImageB_.material.GetFloat("alpha") * 100.0f) / 100.0f >= minB_) 
            //{
            //    // 最小値を設定
            //    min = minB_;
            //}    
        }

        float alpha = (Mathf.Abs(Mathf.Sin(Time.time - startB_)) * maxB_);
        //if (alpha <= min)
        //{
        //    alpha = min;
        //}
        rawImageB_.material.SetFloat("alpha", alpha);

        Debug.Log(rawImageB_.material.GetFloat("alpha"));
    }

    public void DiscoveryNoise()
    {
        rawImageSN_.material.SetFloat("flag", 1.0f);
        startSN_ = Time.time;
    }

    public float GetMoveTimeSN()
    {
        return moveTimeSN_;
    }

    public void SetParameter(float parameter)
    {
        parameter_ = parameter;
    }
}
