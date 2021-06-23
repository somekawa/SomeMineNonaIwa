using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public tBatteryScript batteryScript;
    public Text batteryText;

    private bool pauseFlag;

    // クリアシーンで使うため分と秒はpublicに
    public static int minute;           // 何分か
    public static int seconds;         // 何秒か
    public Text timerText;        // タイマー表示用テキスト
    private bool secondsAddFlag_; // 秒数が足されたか

    void Start()
    {
        minute = 0;
        seconds = 0;
        secondsAddFlag_ = false;
        StartCoroutine("Coroutine");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(pauseFlag != true)
            {
                pauseFlag = true;
            }
            else
            {
                pauseFlag = false;
            }
        }
        batteryText.text = "電池残量" + batteryScript.ReturnBatteryRest()+"%";

        // 毎フレーム呼ばれないようにフラグで管理
        if (secondsAddFlag_ == true)
        {
            // count_60以上＝1分
            if (seconds >= 60f)
            {
                minute++;
                seconds = seconds - 60;
            }
            StartCoroutine("Coroutine");
            secondsAddFlag_ = false;
        }
        timerText.text = minute.ToString("00") + ":" + (seconds).ToString("00");
        Debug.Log("Coroutine"+ seconds);
    }

    public bool GetPauseFlag()
    {
        return pauseFlag;
    }

    private IEnumerator Coroutine()
    {
        //処理１
        Debug.Log("Start");

        //１秒待機
        // WaitForSecondsRealtimeにすることでTimeScaleの影響を受けない
        yield return new WaitForSecondsRealtime(1.0f);
        seconds++;        // 1秒経過したらcount_が+1される
        secondsAddFlag_ = true;
    }

}
