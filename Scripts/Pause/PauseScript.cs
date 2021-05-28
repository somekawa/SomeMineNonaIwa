using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;   //　ポーズした時に表示するUI
    [SerializeField]
    private int minute;           // 何分か
    [SerializeField]
    private float seconds;        // 何秒か

    private float oldSeconds_;    // 前のUpdateの時の秒数
    public Text timerText;        // タイマー表示用テキスト
    private bool pauseFlag_;      // true=pause中　false=ゲーム中

    void Start()
    {
        pauseUI.SetActive(false);
        minute = 0;
        seconds = 0.0f;
        oldSeconds_ = 0.0f;
        pauseFlag_ = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            //　ポーズUIのアクティブ状態切り替え
            pauseUI.SetActive(!pauseUI.activeSelf);
            //　ポーズUIが表示されてる時は停止
            if (pauseUI.activeSelf)
            {
                Time.timeScale = 0f;
                pauseFlag_ = true;
                //　ポーズUIが表示されてなければ通常通り進行
            }
            else
            {
                Time.timeScale = 1f;
                pauseFlag_ = false;
            }
            Debug.Log("pauseUIアクティブ"+pauseUI.activeSelf+"(：ゲーム中true 停止中false)");
        }

        if (pauseFlag_ == true)
        {
            Debug.Log("カメラの回転を止めています");
            return;
        }
        else
        {
            // ゲーム中しか入らない　プレイタイムを計る
            TimeCheck();
        }
        oldSeconds_ = seconds;

    }

    private void TimeCheck()
    {
        // pauseの時以外は時間経過させる
        seconds += Time.deltaTime;
        if (seconds >= 60f)
        {
            minute++;
            seconds = seconds - 60;
        }
        //　値が変わった時だけテキストUIを更新
        if ((int)seconds != (int)oldSeconds_)
        {
            timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
        }
    }

    public bool SetPauseFlag()
    {
        // マウス処理はpause中でも入ってしまうためフラグで管理
        return pauseFlag_;
    }
}
