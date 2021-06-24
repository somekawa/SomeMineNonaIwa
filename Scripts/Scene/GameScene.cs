using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public tBatteryScript batteryScript;
    public Text batteryText;
    public Text keyText;                         // 現在取得している鍵の数
    public PlayerCollision playerColScript;      // itemhitareaのスクリプト
    public Text timerText;                       // タイマー表示用テキスト

    private bool pauseFlag_;
    private int minute_;                          // 何分か
    private int seconds_;                        // 何秒か
    private bool secondsAddFlag_;                // 秒数が足されたか
    private bool resetFlag_;                     // 経過時間をリセットして良いか

    void Start()
    {
        seconds_ = 0;
        secondsAddFlag_ = false;
        resetFlag_ = false;
        StartCoroutine("Coroutine");
    }

    // Update is called once per frame
    void Update()
    {
        //SceneManager.LoadScene("ClearScene");
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(pauseFlag_ != true)
            {
                pauseFlag_ = true;
            }
            else
            {
                pauseFlag_ = false;
            }
        }
        batteryText.text = "電池残量" + batteryScript.ReturnBatteryRest()+"%";

        // チュートリアルからメインに移った時だけはいる
        if (resetFlag_==false&& SceneManager.GetActiveScene().name == "MainScene")
        {
            resetFlag_ = true;
            seconds_ = 0;
            StartCoroutine("Coroutine");
        }

        // 毎フレーム呼ばれないようにフラグで管理
        if (secondsAddFlag_ == true)
        {
            // count_60以上＝1分
            if (seconds_ >= 60f)
            {
                minute_++;
                seconds_ = seconds_ - 60;
            }
            StartCoroutine("Coroutine");
            secondsAddFlag_ = false;
        }
        timerText.text = minute_.ToString("00") + ":" + (seconds_).ToString("00");
        Debug.Log("Coroutine"+ seconds_);
    }

    public bool GetPauseFlag()
    {
        return pauseFlag_;
    }

    private IEnumerator Coroutine()
    {
        //処理１
        Debug.Log("Start");

        //１秒待機
        // WaitForSecondsRealtimeにすることでTimeScaleの影響を受けない
        yield return new WaitForSecondsRealtime(1.0f);
        seconds_++;        // 1秒経過したらcount_が+1される
        secondsAddFlag_ = true;
    }
}
