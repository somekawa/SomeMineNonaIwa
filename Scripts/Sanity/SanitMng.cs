using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SanitMng : MonoBehaviour
{
    public GameObject spotLight_;
    public NoiseControl noiseControl_;
    public GameObject text_;

    private float maxSanit_       = 100.0f;         // 最大正気度
    private float sanit_          =   0.0f;         // 正気度

    private bool oldLightFlag_;
    private float onTime_;                          // 懐中電灯をオンにした時間
    private float offTime_;                         // 懐中電灯をオフにした時間
    public float d_timeMax_;                        // 懐中電灯をオフにしてから耐久出来る最大時間
    public float d_timeMin_;                        // 懐中電灯をオフにしてから耐久出来る最小時間
    private float d_time_;                           // 懐中電灯をオフにしてから耐久出来る実際の時間
    private float d_recoveryTime_ = 10.0f;          // 耐久時間回復にかかる時間
    private float d_nowTime_;                       // 耐久出来る残り時間(確認用のため削除する予定)

    // Start is called before the first frame update
    void Start()
    {
        //text_ = gameObject.transform.Find("SanitCanvas/text").gameObject;

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

        bool lightFlag = spotLight_.activeSelf;
        if (oldLightFlag_ != lightFlag)
        {
            if (!lightFlag)
            {
                onTime_ = 0.0f;
                offTime_ = Time.time;
            }
            else
            {
                if ((offTime_ != 0.0f) && (Time.time - offTime_ > d_time_))
                {
                    // 正気度低下した場合は耐久時間を少なくする
                    d_time_ = d_timeMin_;
                }
                onTime_ = Time.time;
                offTime_ = 0.0f;
            }
        }
        else
        {
            if (!oldLightFlag_) 
            {

                if (offTime_ != 0.0f)
                {
                    d_nowTime_ = d_time_ - (Time.time - offTime_);
                }

                if ((offTime_ != 0.0f) && (Time.time - offTime_ > d_time_))
                {
                    sanit_ -= 0.1f;
                    d_nowTime_ = 0.0f;
                }
            }
            else
            {
                // 耐久出来る時間を回復
                if (d_time_ < d_timeMax_) 
                {
                    d_time_ = (((d_timeMax_ - d_timeMin_) / d_recoveryTime_) * (Time.time - onTime_)) + d_timeMin_;
                }
                else
                {
                    d_time_ = d_timeMax_;
                }

                d_nowTime_ = d_time_;
            }
        }

        oldLightFlag_ = lightFlag;
    }
}
