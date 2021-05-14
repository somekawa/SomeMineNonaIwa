using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SanitMng : MonoBehaviour
{
    public GameObject spotLight_;
    public NoiseControl noiseControl_;
    public HideControl hideControl_;
    public GameObject text_;

    private float maxSanit_       = 100.0f;         // 最大正気度
    // 別シーンに持っていくためにpublic staticに変更
    public static float sanit_    = 0.0f;         // 正気度 

    private bool oldLightFlag_;
    private float onTime_         = 0.0f;           // 懐中電灯をオンにした時間
    private float offTime_        = 0.0f;           // 懐中電灯をオフにした時間
    public float d_timeMax_;                        // 懐中電灯をオフにしてから耐久出来る最大時間
    private float d_time_;                          // 懐中電灯をオフにしてから耐久出来る実際の時間
    public float d_recoveryTime_;                   // 耐久時間1秒回復にかかる時間
    private float d_nowTime_;                       // 耐久出来る残り時間

    private bool recoveryFlag_ = false;             // 回復中

    // Start is called before the first frame update
    void Start()
    {
        oldLightFlag_ = !spotLight_.activeSelf;
        sanit_ = maxSanit_;
        d_time_ = d_timeMax_;
        d_nowTime_ = d_time_;
    }

    // Update is called once per frame
    void Update()
    {
        LightCheck();

        if (sanit_ < 0.0f) 
        {
            sanit_ = 0.0f;
        }

        float parameter = (maxSanit_ - sanit_) * 0.01f;
        noiseControl_.SetParameter(parameter);


        // 確認用
        text_.GetComponent<Text>().text = "耐久時間:" + d_nowTime_;

    }

    private void LightCheck()
    {
        if(spotLight_ == null)
        {
            Debug.Log("spotLight_が入っていません");
            return;
        }

        if(hideControl_.GetHideFlg())
        {
            // 箱の中に隠れている
            DurableRecovery();
            DurableReset();

            return;
        }

        bool lightFlag = spotLight_.activeSelf;
        if (oldLightFlag_ != lightFlag)
        {
            if (lightFlag)
            {
                DurableReset();
            }
            else
            {
                d_time_ = d_nowTime_;
                onTime_ = 0.0f;
                offTime_ = Time.time;
                recoveryFlag_ = false;
            }
        }
        else
        {
            if (oldLightFlag_) 
            {
                DurableRecovery();
            }
            else
            {
                if (offTime_ != 0.0f)
                {
                    d_nowTime_ = d_time_ - (Time.time - offTime_);
                }

                if ((offTime_ != 0.0f) && (d_nowTime_ <= 0.0f)) 
                {
                    sanit_ -= 0.1f;
                    d_nowTime_ = 0.0f;
                }
            }
        }

        oldLightFlag_ = lightFlag;
    }

    // 耐久時間のリセット
    private void DurableReset()
    {
        if (recoveryFlag_) 
        {
            return;
        }

        d_time_ = d_nowTime_;
        if (d_time_ < d_timeMax_)
        {
            recoveryFlag_ = true;
        }
        onTime_ = Time.time;
        offTime_ = 0.0f;
    }

    // 耐久時間の回復
    private void DurableRecovery()
    {
        if(!recoveryFlag_)
        {
            return;
        }

        // 耐久出来る時間を回復
        if (d_nowTime_ < d_timeMax_)
        {
            d_nowTime_ = ((Time.time - onTime_) / d_recoveryTime_) + d_time_;
        }
        else
        {
            d_nowTime_ = d_timeMax_;         
        }
    }
}
