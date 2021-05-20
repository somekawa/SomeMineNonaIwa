﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEditor.SceneManagement;

public class SanitMng : MonoBehaviour
{
    public GameObject spotLight_;
    public NoiseControl noiseControl_;
    public HideControl hideControl_;
    public tLightRange tLightRange_;
    private GameObject text_;

    private float maxSanit_       = 100.0f;         // 最大正気度
    // 別シーンに持っていくためにpublic staticに変更
    public static float sanit_    = 0.0f;           // 正気度 

    private bool gameOvreFlag_    = false;

    private bool loghtDecrease_   = false;          // ライトによる正気度減少
    private bool oldLightFlag_;
    private float onTime_         = 0.0f;           // 懐中電灯をオンにした時間
    private float offTime_        = 0.0f;           // 懐中電灯をオフにした時間
    public float d_timeMax_;                        // 懐中電灯をオフにしてから耐久出来る最大時間
    private float d_time_;                          // 懐中電灯をオフにしてから耐久出来る実際の時間
    public float d_recoveryTime_;                   // 耐久時間1秒回復にかかる時間
    private float d_nowTime_;                       // 耐久出来る残り時間

    private bool enemyDecrease_   = false;          // 敵による正気度減少
    private float enemyHitTime_;

    private bool recoveryFlag_    = false;          // 回復中

    private bool noisFlag_        = false;

    // Start is called before the first frame update
    void Start()
    {
        oldLightFlag_ = !spotLight_.activeSelf;
        sanit_ = maxSanit_;
        d_time_ = d_timeMax_;
        d_nowTime_ = d_time_;

        text_= gameObject.transform.Find("SanitCanvas/Text").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (sanit_ <= 0.0f)
        {
            GameOverSetAction();
        }
        else 
        {
            // ライト
            LightCheck();
            // 敵
            EnemyCheck();
        }

        float parameter = (maxSanit_ - sanit_) * 0.01f;
        noiseControl_.SetParameter(parameter);


        // 確認用
        text_.GetComponent<Text>().text = "耐久時間:" + d_nowTime_;

    }

    private void LightCheck()
    {
        if (sanit_ <= 0.0f) 
        {
            return;
        }

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
                    OffAction();
                }
            }
        }

        oldLightFlag_ = lightFlag;
    }

    private void EnemyCheck()
    {
        if (sanit_ <= 0.0f)
        {
            return;
        }

        if ((!tLightRange_.GetHitCheck())||(!spotLight_.activeSelf))
        {
            // 遭遇していない
            // または懐中電灯をつけていない
            enemyHitTime_ = 0.0f;
            if (!loghtDecrease_)
            {
                noisFlag_ = false;
            }
            enemyDecrease_ = false;
            return;
        }

        if (!noisFlag_)
        {
            noiseControl_.DiscoveryNoise();
            enemyHitTime_ = Time.time;
            noisFlag_ = true;
        }

        if((Time.time - enemyHitTime_)>= 0.0f)
        {
            sanit_ -= 1.0f;
            enemyDecrease_ = true;
        }

    }

    private void OffAction()
    {
        d_nowTime_ = d_time_ - (Time.time - offTime_);

        if ((!noisFlag_) && (d_nowTime_ <= noiseControl_.GetMoveTimeSN()))
        {
            noiseControl_.DiscoveryNoise();
            noisFlag_ = true;
        }

        if (d_nowTime_ <= 0.0f)
        {
            sanit_ -= 0.1f;
            d_nowTime_ = 0.0f;
            loghtDecrease_ = true;
        }
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
        if (!enemyDecrease_)
        {
            noisFlag_ = false;
        }
        onTime_ = Time.time;
        offTime_ = 0.0f;
        loghtDecrease_ = false;
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

    public void GameOverSetAction()
    {
        if(gameOvreFlag_)
        {
            return;
        }

        noiseControl_.DiscoveryNoise();
        sanit_ = 0.0f;
        gameOvreFlag_ = true;

        EditorSceneManager.LoadScene("GameOvreScene");
    }
}
