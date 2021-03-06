using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoiseControl : MonoBehaviour
{
    public float parameter          = 0.0f;     // パラメーター(0.0f～1.0f)

    // ノイズ
    public float maxN;                          // ノイズの最大値
    private RawImage rawImageN_;                

    // 血
    public float maxB;                          // 血の最大値
    private RawImage rawImageB_;               
    private float minB_             = 0.0f;     // 血の最小値
    private float startB_           = 0.0f;     // 血出現開始時間
    private const float parameterB_ = 0.8f;     // 血出現開始条件(パラメーター)
    private bool useB_              = false;    // 表示中

    // 横ノイズ
    public float moveTimeSN;                    // 稼働時間
    private RawImage rawImageSN_;               
    private float startSN_          = 0.0f;     // 横ノイズ開始時間
    private bool useSN_             = false;    // 表示中
    private bool randomSN_          = false;    // ランダムに表示する(ゲームオーバー時使用)

    // スレンダーマン画像
    private RawImage rawImageS_;
    private float startSI_          = 0.0f;     // スレンダーマン表示開始時間
    private float moveTimeSI_       = 1.5f;     // 一度に表示する時間(endless_がtrueの時を除く)
    private bool useSI_             = false;    // 表示中

    // パルスノイズ
    private float parameterPN_      = 0.0f;
    private float coolTimePN_       = 0.0f;
    private bool usePN_             = false;    // 表示中

    private bool endless_           = false;    // 時間関係なく流し続ける

    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        RawImage[] rawImageList =gameObject.GetComponentsInChildren<RawImage>();
        foreach(RawImage rawImage in rawImageList)
        {
            rawImage.rectTransform.sizeDelta = screenSize;
            if (rawImage.name == "N_RawImage")
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
            else if (rawImage.name == "S_RawImage")
            {
                // スレンダーマン画像
                rawImageS_ = rawImage;
                if (rawImageS_.gameObject.activeSelf)
                {
                    // 初期時は非表示にする
                    rawImageS_.gameObject.SetActive(false);
                }
            }
            else
            {
                // 何もしない
            }
        }

        if(SceneManager.GetActiveScene().name == "GameOverScene")
        {
            randomSN_ = true;
        }

        useB_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        // 横ノイズのランダム表示
        RandomSN();

        // ノイズ
        if (parameter <= 1.0f)
        {
            float alpha = maxN;
            if(!useSN_)
            {
                alpha = parameter * maxN;
            }
            rawImageN_.material.SetFloat("alpha", alpha);
        }
        rawImageN_.material.SetFloat("time", Time.time);

        // 血(0.8以上から出現)
        BloodUpdate();

        // スレンダーマン画像
        SIUpdate();

        // パルスノイズ
        PulseNoiseUpdate();     

        // 横ノイズ
        if (rawImageSN_.material.GetFloat("flag") != 0.0f)
        {
            if(startSN_==0.0f)
            {
                startSN_ = Time.time;
            }
            rawImageSN_.material.SetFloat("time", Time.time);
            if ((!endless_) && (moveTimeSN < Time.time - startSN_)) 
            {
                rawImageSN_.material.SetFloat("flag", 0.0f);
                rawImageS_.gameObject.SetActive(false);
                startSN_ = 0.0f;
                useSN_ = false;
            }
        }
    }

    private void BloodUpdate()
    {
        if (parameter < 0.8f)
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
                useB_ = true;
                startB_ = Time.time;
            }
        }

        if (SceneManager.GetActiveScene().name == "MainScene")
        {
            minB_ = (0.4f / 0.2f) * (parameter - 0.8f);
        }

        float alpha = (Mathf.Abs(Mathf.Sin(Time.time - startB_)) * maxB) + minB_;

        rawImageB_.material.SetFloat("alpha", alpha);
        Debug.Log(rawImageB_.material.GetFloat("alpha"));
    }

    private void SIUpdate()
    {
        if ((endless_)||                                            // ゲームオーバー時に使用
            (useSI_) ||                                         // 使用済み
            (SceneManager.GetActiveScene().name != "MainScene")||   // ゲーム中ではない
            (parameter < 0.8f))                                     // 対応正気度でない
        {
            // 正気度が戻った場合はリセットする
            if (parameter < 0.8f)
            {
                useSI_ = false;
            }

            return;
        }

        Color color = rawImageS_.color;
        if ((startSI_ != 0.0f) && (Time.time - startSI_ >= moveTimeSI_))
        {
            rawImageS_.gameObject.SetActive(false);

            color.a = 1.0f;
            rawImageS_.material.SetColor("color_", color);

            useSI_ = true;                          // 使用済み
            return;
        }

        if(startSI_ == 0.0f)
        {
            startSI_ = Time.time;
        }

        rawImageS_.gameObject.SetActive(true);
        color.a = 0.4f;
        rawImageS_.material.SetColor("color_", color);
    }

    private void PulseNoiseUpdate()
    {
        if (!rawImageS_.gameObject.activeSelf)
        {
            return;
        }

        if (!usePN_)
        {
            coolTimePN_ -= Time.deltaTime;
            if (coolTimePN_ > 0.0f)
            {
                return;
            }
            usePN_ = true;
        }

        rawImageS_.material.SetFloat("amount_", 0.5f * Mathf.Sin(parameterPN_ * Mathf.Deg2Rad));
        parameterPN_ += 30.0f;

        if (parameterPN_ > 180.0f) 
        {
            parameterPN_ = 0.0f;
            coolTimePN_  = Random.value;
            usePN_   = false;
        }
    }

    // 横ノイズを永遠に流す(ゲームオーバー移行時に使用)
    public void DiscoveryNoiseEndless(bool slenderFlag)
    {
        DiscoveryNoise(slenderFlag);
        endless_ = true;
    }

    // 横ノイズをランダムに流す(ゲームオーバー時使用)
    private void RandomSN()
    {
        if (!randomSN_)
        {
            return;
        }

        if (Random.value * 100.0f < 0.1f)
        {
            DiscoveryNoise(false);
        }
    }

    public void DiscoveryNoise(bool slenderFlag)
    {
        rawImageSN_.material.SetFloat("flag", 1.0f);

        if (slenderFlag)
        {
            Color color = rawImageS_.color;
            color.a = 1.0f;
            rawImageS_.material.SetColor("color_", color);
            rawImageS_.gameObject.SetActive(true);
        }        
        startSN_ = Time.time;
        useSN_ = true;
    }

    public float GetMoveTimeSN()
    {
        return moveTimeSN;
    }

    public void SetParameter(float parameter)
    {
        this.parameter = parameter;
    }

    public bool GetBRawImage()
    {
       return useB_;      
    }
}
