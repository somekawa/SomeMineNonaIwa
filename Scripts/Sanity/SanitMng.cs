using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SanitMng : MonoBehaviour
{
    // 死因
    public enum DeadType
    {
        NON,
        SANIT,      // 正気度が0
        HIT,        // 敵に当たった
        MAX
    }

    public GameScene gameScene_;
    public GameObject spotLight_;                       // ライト
    public NoiseControl noiseControl_;                  // ノイズ
    public HideControl hideControl_;                    // 箱に隠れる処理
    public tLightRange tLightRange_;                    // ライトのあたり判定

    private const float maxSanit_    = 100.0f;          // 最大正気度
    public static float sanit_;                         // 正気度(100から0に減少していく) 

    private bool loghtDecrease_      = false;           // ライトによる正気度減少
    private bool oldLightFlag_;                         // 前回のライトの状態
    private float onTime_            = 0.0f;            // 懐中電灯をオンにした時間
    private float offTime_           = 0.0f;            // 懐中電灯をオフにした時間
    public float d_timeMax_;                            // 懐中電灯をオフにしてから耐久出来る最大時間
    private float d_time_;                              // 懐中電灯をオフにしてから耐久出来る実際の時間
    public float d_recoveryTime_;                       // 耐久時間1秒回復にかかる時間
    private float d_nowTime_;                           // 耐久出来る残り時間

    private bool enemyDecrease_      = false;           // 敵による正気度減少
    private float enemyHitTime_;

    private bool recoveryFlag_       = false;           // 回復中

    private bool noisFlag_           = false;           // ノイズが走っているか

    public static DeadType deadType_ = DeadType.NON;

    private float rTime_;                               // 再生時間
    private float rMaxTime_          = 3.0f;

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
        if(gameScene_.GetPauseFlag())
        {
            // ポーズ中
            return;
        }

        if (!gameScene_.GetGameOverFlag())
        {
            if (sanit_ <= 0.0f)
            {
                GameOverSetAction(DeadType.SANIT);
            }
            else
            {
                // ライト
                LightCheck();
                // 敵
                EnemyCheck();
            }
        }
        else
        {
            // ゲームオーバー演出
            rTime_ += Time.deltaTime;
            if (rTime_ >= rMaxTime_) 
            {
                SoundScript.GetInstance().audioSourceSE.Stop();

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene("GameOverScene");
            }
        }

		// ノイズ用に値を変換
        float parameter = (maxSanit_ - sanit_) * 0.01f;
        noiseControl_.SetParameter(parameter);
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

        // 懐中電灯がオフになった時
        if ((!lightFlag) && (offTime_ == 0.0f)) 
        {
            d_time_ = d_nowTime_;
            onTime_ = 0.0f;
            offTime_ = Time.time;
            recoveryFlag_ = false;
        }

        if (oldLightFlag_ != lightFlag)
        {
            // 懐中電灯をオンにした時
            if (lightFlag)
            {
                DurableReset();
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
            noiseControl_.DiscoveryNoise(true);
            enemyHitTime_ = Time.time;
            noisFlag_ = true;
        }

        if((Time.time - enemyHitTime_)>= 0.0f)
        {
            sanit_ -= 0.5f;
            enemyDecrease_ = true;
        }

    }

    // 懐中電灯オフによる正気度減少処理
    private void OffAction()
    {
        d_nowTime_ = d_time_ - (Time.time - offTime_);

        if ((!noisFlag_) && (d_nowTime_ <= noiseControl_.GetMoveTimeSN()))
        {
            noiseControl_.DiscoveryNoise(false);
            noisFlag_ = true;
        }

        if (d_nowTime_ <= 0.0f)
        {
            float sanit = 0.1f;
            if (sanit_ <= 20.0f) 
            {
                sanit = 0.02f;
            }
            sanit_ -= sanit;
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

    public void GameOverSetAction(DeadType deadType)
    {
        if(gameScene_.GetGameOverFlag())
        {
            return;
        }
        noiseControl_.DiscoveryNoiseEndless(true);
        tLightRange_.gameObject.SetActive(false);
        rTime_ = 0.0f;
        deadType_ = deadType;
        sanit_ = 0.0f;
        gameScene_.SetGameOverFlag(true);

        SoundScript.GetInstance().PlaySound(6);
    }

    public float GetDTimeMax()
    {
        return d_timeMax_;
    }

    public float GetDNTimeMax()
    {
        return d_nowTime_;
    }
}
