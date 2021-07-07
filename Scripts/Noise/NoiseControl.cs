using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool sNFlag_            = false;

    private Image slenderImage_;
    private bool useFlagSI_ = false;
    private float startSI_          = 0.0f;     // スレンダーマン表示開始時間
    private float moveTimeSI_       = 1.0f;

    private bool endless_           = false;    // 時間関係なく流し続ける
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        RawImage[] rawImageList =gameObject.GetComponentsInChildren<RawImage>();
        foreach(RawImage rawImage in rawImageList)
        {
            rawImage.rectTransform.sizeDelta = screenSize;
            if (rawImage.name== "N_RawImage")
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

        slenderImage_ = gameObject.GetComponentInChildren<Image>();
        slenderImage_.rectTransform.sizeDelta = screenSize;
        if(slenderImage_.gameObject.activeSelf)
        {
            // 初期時は非表示にする
            slenderImage_.gameObject.SetActive(false);
        }        //minB_ = maxB_ - 1.0f;
    }

    // Update is called once per frame
    void Update()
    {

        // ノイズ
        if (parameter_ <= 1.0f)
        {
            float alpha = maxN_;
            if(!sNFlag_)
            {
                alpha = parameter_ * maxN_;
            }
            rawImageN_.material.SetFloat("alpha", alpha);
        }
        rawImageN_.material.SetFloat("time", Time.time);

        // 血(0.8以上から出現)
        BloodUpdate();

        SIUpdate();

        // 横ノイズ
        if (rawImageSN_.material.GetFloat("flag")!=0.0f)
        {
            if(startSN_==0.0f)
            {
                startSN_ = Time.time;
            }
            rawImageSN_.material.SetFloat("time", Time.time);
            if ((!endless_) && (moveTimeSN_ < Time.time - startSN_)) 
            {
                rawImageSN_.material.SetFloat("flag", 0.0f);
                slenderImage_.gameObject.SetActive(false);
                startSN_ = 0.0f;
                sNFlag_ = false;
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
        }

        float alpha = (Mathf.Abs(Mathf.Sin(Time.time - startB_)) * maxB_);
        rawImageB_.material.SetFloat("alpha", alpha);

        Debug.Log(rawImageB_.material.GetFloat("alpha"));
    }

    private void SIUpdate()
    {
        if ((useFlagSI_) ||                                         // 使用済み
            (SceneManager.GetActiveScene().name != "MainScene")||   // ゲーム中ではない
            (parameter_ < 0.8f))                                    // 対応正気度でない
        {
            // 正気度が戻った場合はリセットする
            if (parameter_ < 0.8f)
            {
                useFlagSI_ = false;
            }

            return;
        }

        Color color = slenderImage_.color;
        if (((startSI_ != 0.0f) && (Time.time - startSI_ >= moveTimeSI_))) 
        {

            slenderImage_.gameObject.SetActive(false);

            color.a = 1.0f;
            slenderImage_.color = color;
            useFlagSI_ = true;                          // 使用済み
            return;
        }

        if(startSI_ == 0.0f)
        {
            startSI_ = Time.time;
        }

        slenderImage_.gameObject.SetActive(true);
        color.a = 0.4f;
        slenderImage_.color = color;
    }

    // 横ノイズを永遠に流す(ゲームオーバー時に使用)
    public void DiscoveryNoiseEndless(bool slenderFlag)
    {
        DiscoveryNoise(slenderFlag);
        endless_ = true;
    }
    public void DiscoveryNoise(bool slenderFlag)
    {
        rawImageSN_.material.SetFloat("flag", 1.0f);

        if (slenderFlag)
        {

            slenderImage_.gameObject.SetActive(true);
        }        
        startSN_ = Time.time;
        sNFlag_ = true;
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
