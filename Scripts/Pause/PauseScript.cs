using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUI;   //　ポーズした時に表示するUI
    //[SerializeField]
    //private int minute;           // 何分か
    //[SerializeField]
    //private float realTimeSeconds;// Time.realtimeSinceStartupを代入


    //// メニュー表示時に時間を止める場合の変数
    //private float seconds;        // 何秒か
    //private float oldSeconds_;    // 前のUpdateの時の秒数

    //public Text timerText;        // タイマー表示用テキスト
    public GameScene gameManager;
    private bool pauseFlag_;      // true=pause中　false=ゲーム中

    //// メニュー表示時でも時間経過させるための変数
    //// realTimeSeconds
    //private float numSixty_;
    //private float secondsMinus_;

    //public Text test;
    void Start()
    {
        pauseUI.SetActive(false);
        //minute = 0;
        //numSixty_ = 60.0f;
        //secondsMinus_ = 0.0f;
        //realTimeSeconds = 0.0f;
        //seconds = 0.0f;
        //oldSeconds_ = 0.0f;
        pauseFlag_ = false;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Tab))
        //{
        //    //　ポーズUIのアクティブ状態切り替え
        //    pauseUI.SetActive(!pauseUI.activeSelf);
        //    //　ポーズUIが表示されてる時は停止
        //    if (pauseUI.activeSelf)
        //    {
        //        Time.timeScale = 0f;
        //        pauseFlag_ = true;
        //        //　ポーズUIが表示されてなければ通常通り進行
        //    }
        //    else
        //    {
        //        Time.timeScale = 1f;
        //        pauseFlag_ = false;
        //    }
        //    Debug.Log("pauseUIアクティブ"+pauseUI.activeSelf+"(：ゲーム中true 停止中false)");
        //}

        if (gameManager.GetPauseFlag() == true)
        {
            //　ポーズUIのアクティブ状態切り替え
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
            pauseFlag_ = true;
            Debug.Log("pauseUIアクティブ" + pauseUI.activeSelf + "(：ゲーム中true 停止中false)");
        }
        else
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
            pauseFlag_ = false;
            //  TimeCheck();
            Debug.Log("pauseUIアクティブ" + pauseUI.activeSelf + "(：ゲーム中true 停止中false)");
        }

        //TimeCheck();
        ////oldSeconds_ = seconds;
        ////Debug.Log("Time.timeScale" + Time.timeScale);

    }

    //private void TimeCheck()
    //{
    //    /* pauseの時以外は時間経過させる*/
    //    // seconds += Time.deltaTime;
    //    // Debug.Log("Time.realtimeSinceStartup" + tes);
    //    // Debug.Log("tes+Time.time" + tes);
    //    //        if (seconds >= 60f)
    //    //        {
    //    //            minute++;
    //    //            seconds = seconds - 60;
    //    //        }
    //    //        //　値が変わった時だけテキストUIを更新
    //    //        if ((int)seconds != (int)oldSeconds_)
    //    //        {
    //    ////            timerText.text = minute.ToString("00") + ":" + ((int)seconds).ToString("00");
    //    //        }
    //    /*ここまで*/


    //    ///*メニュー表示中も時間を経過させたい*/
    //    //realTimeSeconds = Time.realtimeSinceStartup;
    //    //// 経過時間÷60が分+1より大きかったら分を加算する
    //    //if (realTimeSeconds / numSixty_ >= minute + 1)
    //    //{
    //    //    // minuteの初期値は0のため
    //    //    minute++;
    //    //    // tesは毎フレーム更新するため60秒、120秒になった時のマイナス分
    //    //    secondsMinus_ += 60;
    //    //}

    //    //timerText.text = minute.ToString("00") + ":" + ((int)realTimeSeconds - secondsMinus_).ToString("00");
    //    ////Debug.Log("realTimeSeconds"+ realTimeSeconds);
    //    ///*ここまで*/
    //}

    public bool GetPauseFlag()
    {
        // マウス処理はpause中でも入ってしまうためフラグで管理
        return pauseFlag_;
    }

    public void TransitionTitle()
    {
        SceneManager.LoadScene("TitleSample");
        Time.timeScale = 1f;
    }
}
