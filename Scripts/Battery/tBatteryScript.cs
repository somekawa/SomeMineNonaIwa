using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tBatteryScript : MonoBehaviour
{
    public tLightScript lightScript;
    public PlayerCollision playerCollision;
    public HideControl hideCtl;

    // lightONの時に消費する量
    public float erasePoint = 0.03f;        // 1秒間に減る量（0.03f）
    // 電池を拾ったときに使う変数
    private float chargingNum_ = 0.8f;      // チャージ量（0.8f）
    private float overBattery_ = 0.0f;      // 右の電池を充電した際に1.0を超えた分を保存する

    // 電池が消費する時間を計る
    private float countdown_ = 1.0f;
   
    // デバッグ用変数
    private int count_;         // 1秒カウントダウンするたびにカウントする

    // 電池のタイプ
    public enum type
    {
        LEFT,   // 左の電池
        RIGHT,  // 右の電池
        MAX,
    }

    // 電池の状態
    struct statusBattery
    {
        public Image batteryImage;  // どの画像であるか
        public float max;           // 充電の最大値=1.0f
        public float min;           // 充電の最低値=0.0f
        public float danger;        // どの充電量で危険を知らせるか
        public float save;          // 現在の充電量を保存
    }
    private statusBattery[] statusBattery_;

    void Start()
    {
        statusBattery_ = new statusBattery[(int)type.MAX];
        Debug.Log("バッテリーの数"+this.transform.childCount);
        for (int i = 0; i < (int)type.MAX ; i++) 
        {
            Debug.Log((int)type.MAX + "回中" + i + "回目");// 各電池の情報を初期化
            //// 各電池の情報を初期化
            statusBattery_[i] = new statusBattery()
            {
                batteryImage = this.transform.GetChild(i).GetComponent<Image>(),
                max = 1.0f,
                min = 0.0f,
                save = this.transform.GetChild(i).GetComponent<Image>().fillAmount,
            };
        }
        statusBattery_[(int)type.LEFT].danger = 0.0f;
        statusBattery_[(int)type.RIGHT].danger = 0.3f;
        //Debug.Log("左電池" + status_[(int)type.LEFT].batteryImage +
        //    "右電池" + status_[(int)type.RIGHT].batteryImage);

        // デバッグ用
        count_ = 0;
    }

    void Update()
    {
        if (hideCtl.GetHideFlg() == true)
        {
            // 箱に隠れている時は電池を消費しないようにする
            return;
        }
        countdown_ -= Time.deltaTime;        // カウントダウンする
      
        // 両方の充電がないとき
        if (statusBattery_[(int)type.RIGHT].batteryImage.fillAmount <= statusBattery_[(int)type.RIGHT].min)
        {            
            // 両方の電池が0になった
            lightScript.GetAccidentFlag(true);// 強制的にライトオフ
            statusBattery_[(int)type.RIGHT].batteryImage.fillAmount = statusBattery_[(int)type.RIGHT].min;
        }
        else  // 電池があるとき
        {
            // ライトがオンの時だけ　ゲージが減る
            if (lightScript.lightStatus == tLightScript.light_Status.ON)
            {
                // カウントが0以下になったら関数を抜ける
                if (0 < countdown_) 
                {
                    return;
                }
                countdown_ = 1.0f;   // 1秒を繰り返させる
                count_++;            // 1秒ごとにカウントを追加して何秒経過してるかを確認する

                // 左側の電池があるとき　左側の電池消費中
                if (statusBattery_[(int)type.LEFT].min < statusBattery_[(int)type.LEFT].batteryImage.fillAmount)
                {
                    // カウント0以下＝1秒経過
                    UsingBattery(type.LEFT);
                }
                else                    // gauge1が0になったらgauge2を減らし始める
                {
                    UsingBattery(type.RIGHT);
                }
            }
        }

        if (playerCollision.GetBatteryFlag() == true)
        {
            //プレイヤーが電池を拾ったら充電をする
            BatteryCharging();
            playerCollision.SetBatteryFlag(false);
        }

        // 各電池の充電量を保存
        statusBattery_[(int)type.LEFT].save = statusBattery_[(int)type.LEFT].batteryImage.fillAmount;
        statusBattery_[(int)type.RIGHT].save = statusBattery_[(int)type.RIGHT].batteryImage.fillAmount;
    }

    private void UsingBattery(type type_)
    {
        // ライトがついてる間だけ電池残量が減る 1秒間に電池が減る度合い
        statusBattery_[(int)type_].batteryImage.fillAmount -= erasePoint;

        if (type_ != type.RIGHT)
        {
            return;            // RIGHTじゃなかったらこの行以下の処理を行わない
        }

        // type_内にRIGHT以外だとreturnのため　type_=RIGHTになってる
        // ↓右のBatteryのfillAmountが残りが0.03f以下になったら赤いゲージを表示する
        if (statusBattery_[(int)type_].batteryImage.fillAmount <= statusBattery_[(int)type_].danger)
        {
            statusBattery_[(int)type_].batteryImage.color = Color.red;
        }
        else
        {
            statusBattery_[(int)type_].batteryImage.color = new Color(255, 255, 0, 1.0f);
        }

        // 右が呼ばれたら左の値に最小値を代入
        statusBattery_[(int)type.LEFT].batteryImage.fillAmount = statusBattery_[(int)type.LEFT].min;
    }
    
    private void BatteryCharging()
    {
        if (lightScript.SetAccidentFlag() == true)
        {
            // アクシデント＝両方の充電が0になったときに回復した場合
            // 1.0以上の回復はないため左の電池の充電の計算はいらない
            statusBattery_[(int)type.RIGHT].batteryImage.color = new Color(255, 255, 0, 1.0f);
            statusBattery_[(int)type.RIGHT].batteryImage.fillAmount += chargingNum_;
            lightScript.GetAccidentFlag(false);            // false=アクシデント終了
        }
        else
        {
            // 左側の充電がないかつ右側の充電が消費されている時=右側の充電がMAXではないとき
            if (statusBattery_[(int)type.LEFT].batteryImage.fillAmount  <= statusBattery_[(int)type.LEFT].min
             && statusBattery_[(int)type.RIGHT].batteryImage.fillAmount < statusBattery_[(int)type.RIGHT].max)  
            {
                ChargeBatteryType(type.RIGHT);
            }
            else if (statusBattery_[(int)type.LEFT].min <= statusBattery_[(int)type.LEFT].save)   
            {
                // 左側の充電がある場合＝右側がMAXの時
                ChargeBatteryType(type.LEFT);
                Debug.Log("左の電池を充電しました");
            }
            else
            {
                return; // 何も処理を行わない
            }
        }
    }

    public void ChargeBatteryType(type type_)
    {
        // 保存した分に電池分を足す
        statusBattery_[(int)type_].save += chargingNum_;
        Debug.Log("アイテムを拾いました");

        // 充電が危険域より多くあるなら通常の色にする
        if (statusBattery_[(int)type_].danger <= statusBattery_[(int)type_].save)
        {
            Debug.Log("危険域から回復しました");
            statusBattery_[(int)type_].batteryImage.color = new Color(255, 255, 0, 1.0f);
        }

        // ↓足した際に右の充電が1.0を超えていなかったら
        if (statusBattery_[(int)type_].save <= statusBattery_[(int)type_].max)
        {
            // 1.0を超えてないならsaveした値をfillAmountに代入
            statusBattery_[(int)type_].batteryImage.fillAmount = statusBattery_[(int)type_].save;
        }
        else
        {
            // MAX(1.0f)を超えたら            // fillAmountが1.0を超えないようにする
            statusBattery_[(int)type_].batteryImage.fillAmount = statusBattery_[(int)type_].max;

            // 代入されるのが右の電池だった場合
            if (type_ != type.RIGHT)
            {
                return;  // RIGHTじゃなかったらこの行以下の処理を行わない
            }

            // 1.0を超えた分をoverに保存
            overBattery_ = statusBattery_[(int)type.RIGHT].save - statusBattery_[(int)type.RIGHT].max;
            // 右電池に最大値を代入
            statusBattery_[(int)type.RIGHT].batteryImage.fillAmount = statusBattery_[(int)type.RIGHT].max;

            // overした分を左の電池に代入
            // 拾った電池は1.0以上がないから　左側が1.0を超えたかどうかの確認はいらない
            statusBattery_[(int)type.LEFT].batteryImage.fillAmount += overBattery_;
            Debug.Log("両方の電池を充電しました");
        }
    }

    public float ReturnBatteryRest()
    {
        float batteryLevel_ = 0;
        batteryLevel_ = ((statusBattery_[(int)type.LEFT].batteryImage.fillAmount + statusBattery_[(int)type.RIGHT].batteryImage.fillAmount) / 2) * 100;
        return batteryLevel_;
    }
}
