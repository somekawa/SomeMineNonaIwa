using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tLightScript : MonoBehaviour
{
    // enumはc++でいうenum class
    public enum light_Status
    {
        ON,
        OFF,
        ACCIDENT,
    }
    public light_Status lightStatus;

    public HideControl hideCtl;

    // 懐中電灯関連
    public GameObject spotLight;    // spotlightを参照するため
    private bool accidentFlag_;// 充電が0になったとき　true＝0になった　false＝0ではない

    // 音楽関連
    private AudioSource audioSource_;
    [SerializeField] private AudioClip normalSE_;
    [SerializeField] private AudioClip accidentSE_;
    private bool seAccidentFlag_;// 電池切れの時1度だけ鳴らすSE　true=なる　false=鳴らさない

    void Start()
    {
        lightStatus = light_Status.ON;

        audioSource_ = GetComponent<AudioSource>();
        // ゲーム開始時にライトはついている
        accidentFlag_ = false;
        seAccidentFlag_ = false;
    }

    void Update()
    {
        if(hideCtl.GetHideFlg()==true)
        {
           // 隠れたときに自動的にライトOFF
            NowLightStatus(light_Status.OFF, false);
        }
        else if (hideCtl.GetHideFlg() == false&& Input.GetKey(KeyCode.F))
        {
            // ボックスから出たとき＝false
            // hideFlagは隠れてないとき基本falseのため、出ていくときの
            // Fキー押下を自動的にオンしているようにみせる
            NowLightStatus(light_Status.ON, true);

        }

        // ライトオンオフ
        if (Input.GetMouseButtonDown(0))
        {
            //// 電池があるとき通常のSE
            if (accidentFlag_ == true)
            {
                // 電池がないとき
                SE();//AccidentSE();
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
                else if(lightStatus == light_Status.OFF)
                {
                    // OFFの時はONにできる　電池が減る
                    NowLightStatus(light_Status.ON, true);
                }
                SE();//NomarlSE(); // 電池があるとき通常のSE
            }
            Debug.Log(lightStatus);
        }

        // 充電がない時用の処理
        NothingBattery();
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
                SE();//AccidentSE();
                Debug.Log("電池残量0：強制OFFのSE");
            }
        }

        // 充電が0で電池を拾った場合
        if (lightStatus == light_Status.ACCIDENT && accidentFlag_ == false)
        {
            SE();// NomarlSE();
            NowLightStatus(light_Status.ON, true);
            Debug.Log("充電が0からかいふくしました");
        }

    }

    private void NowLightStatus(light_Status status, bool active)
    {
        lightStatus = status;            // lightがついてるかのステータス
        spotLight.SetActive(active);     // 表示されるかどうか　false:OFF  true:ON
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

  //  public void NomarlSE()
    public void SE()
    {
        if (accidentFlag_ == false)
        {
            // 充電があるときに懐中電灯ON/OFFのSE
            audioSource_.PlayOneShot(normalSE_);
        }
        else
        {
            // 充電がないときの懐中電灯SE
            audioSource_.PlayOneShot(accidentSE_);

        }
    }

    //public void AccidentSE()
    //{
    //    // 充電がないときの懐中電灯SE
    //    audioSource_.PlayOneShot(accidentSE_);
    //}

}
