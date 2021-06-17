using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    private bool pauseFlag;

    private int minute;           // 何分か
    public Text timerText;        // タイマー表示用テキスト
    private int seconds_;         // 何秒か
    private bool secondsAddFlag_; // 秒数が足されたか
    private bool resetFlag_;      // 経過時間をリセットして良いか

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
                minute++;
                seconds_ = seconds_ - 60;
            }
            StartCoroutine("Coroutine");
            secondsAddFlag_ = false;
        }
        timerText.text = minute.ToString("00") + ":" + (seconds_).ToString("00");
        Debug.Log("Coroutine"+ seconds_);
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
        seconds_++;        // 1秒経過したらcount_が+1される
        secondsAddFlag_ = true;
    }

}
