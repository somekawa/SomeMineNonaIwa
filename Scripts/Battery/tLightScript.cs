using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tLightScript : MonoBehaviour
{
    // enumはc++でいうenum class
    public enum light_Status
    {
        ON,             // ライトがついている時
        OFF,            // ライトが消えている時
        ACCIDENT,       // ライトが強制OFFの時
    }
    public light_Status lightStatus;

    public HideControl hideControl;     // 箱に隠れる処理
    public PauseScript pause;           // pause中の処理
    public GameObject inSightArea;      // 敵の処理を働かせる範囲

    // 懐中電灯関連
    public GameObject spotLight;    // spotlightを参照するため
    private bool accidentFlag_;// 充電が0になったとき　true=0になった　false=0ではない
    private GameScene gameScene_;

    // 音楽関連
    private AudioSource audioSource_;
    [SerializeField] private AudioClip normalSE_;   // 充電があるときに懐中電灯SE
    [SerializeField] private AudioClip accidentSE_; // 充電がないときの懐中電灯SE
   // 電池切れの時1度だけ鳴らすSE　true=なる　false=鳴らさない
    private bool seAccidentFlag_;
    // true=隠れている(音が出ない)　false=隠れる、出る時　ライトの音が出る
    private bool hideSEFlag_;  

    void Start()
    {
        lightStatus = light_Status.ON;
        gameScene_ = FindObjectOfType<GameScene>();

        audioSource_ = GetComponent<AudioSource>();
        // ゲーム開始時にライトはついている
        accidentFlag_ = false;
        seAccidentFlag_ = false;
        hideSEFlag_=false;
    }

    void Update()
    {
        if (gameScene_.GetPauseFlag() == true)
        {
            return;            // pause中は何の処理もできないようにする
        }

        // ライトオンオフ            // 隠れている時は音はならない
        if (Input.GetMouseButtonDown(0) && hideControl.GetHideFlg() == false) 
        {
            //// 電池があるとき通常のSE
            if (accidentFlag_ == true)
            {
                // 電池がないとき
                SE(accidentSE_);//AccidentSE();
                lightStatus = light_Status.ACCIDENT;
                Debug.Log("電池残量0：クリック時SE");
            }
            else
            {
                if (lightStatus == light_Status.ON)
                {
                    // ONの時はOFFにする　電池が減らないようになる
                    NowLightStatus(light_Status.OFF, false);
                }
                else if (lightStatus == light_Status.OFF)
                {
                    // OFFの時はONにできる　電池が減る
                    NowLightStatus(light_Status.ON, true);
                }
                else
                {
                    return;
                }
                SE(normalSE_);//NomarlSE(); // 電池があるとき通常のSE
            }
            Debug.Log(lightStatus);
        }

        // 充電がない時用の処理
        NothingBattery();

        if (Input.GetKey(KeyCode.F))
        {
            // 隠れてる時関連の処理 隠れるときの押下キー
            HideNowLight();
        }
    }

    private void NothingBattery()
    {
        // 電池がなくなったときは強制的にライトをオフにする
        if (accidentFlag_ == true)
        {
            // 電池が0以上にならないとライトはつかない
            NowLightStatus(light_Status.ACCIDENT, false);
            if (seAccidentFlag_ == false)
            {
                seAccidentFlag_ = true;
                SE(accidentSE_);//AccidentSE();
                Debug.Log("電池残量0：強制OFFのSE");
            }
        }

        // 充電が0で電池を拾った場合
        if (lightStatus == light_Status.ACCIDENT && accidentFlag_ == false)
        {
            SE(normalSE_);// NomarlSE();
            NowLightStatus(light_Status.ON, true);
            Debug.Log("充電が0からかいふくしました");

            // 強制OFFが終わったら、また強制offになった時のために
            // アクシデントSEが鳴るようにする
            seAccidentFlag_ = false;
        }

    }

    private void HideNowLight()
    {
        if (hideControl.GetHideFlg() == true)
        {
            // 隠れたときに自動的にライトOFF
            NowLightStatus(light_Status.OFF, false);
            if (hideSEFlag_ == false)
            {
                hideSEFlag_ = true;
                SE(normalSE_);
            }
        }
        else
        {
            if (hideSEFlag_ == true)
            {
                // ボックスから出たとき＝false
                // hideFlagは隠れてないとき基本falseのため、出ていくときの
                // Fキー押下を自動的にオンしているようにみせる
                NowLightStatus(light_Status.ON, true);
                hideSEFlag_ = false;
                SE(normalSE_);
            }
        }
    }

    private void NowLightStatus(light_Status status, bool active)
    {
        lightStatus = status;            // lightがついてるかのステータス
        spotLight.SetActive(active);     // 表示されるかどうか　false:OFF  true:ON
        inSightArea.SetActive(active);
    }

    public void GetAccidentFlag(bool flag)
    {
        // Batteryがあるかのチェック　BatteryScriptで使用
        accidentFlag_ = flag;
    }

    public bool SetAccidentFlag()
    {
        // Batteryがあるかのチェック　BatteryScriptで使用
        return accidentFlag_;
    }

    public void SE(AudioClip audio)
    {
        // SEの音を鳴らす　鳴らしたい音がaudioにはいる
        audioSource_.PlayOneShot(audio);
    }


}
