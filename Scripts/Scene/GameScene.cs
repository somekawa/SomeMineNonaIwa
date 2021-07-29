using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameScene : MonoBehaviour
{
    public tBatteryScript batteryScript;
    public Text batteryText;
    public Text[] keyText;
    public PlayerCollision playerColScript;
    public GameObject collectCanvas;

    private bool pauseFlag_;

    // クリアシーンで使うため分と秒はpublicに
    public static int minute  = 0;          // 何分か
    public static int seconds = 0;          // 何秒か
    public Text timerText;                  // タイマー表示用テキスト
    private bool secondsAddFlag_ = false;   // 秒数が足されたか

    private float startAnimTime_ = 0.0f;

    void Start()
    {
        StartCoroutine("Coroutine");
        collectCanvas.SetActive(false);
    }

    void Update()
    {
        // スタート時のアニメーション中はTABキー操作ができないようにする
        if (startAnimTime_ < 7.0f)
        {
            startAnimTime_ += Time.deltaTime;
        }
        else
        {
            collectCanvas.SetActive(true);
        }

        // ポーズ（メニュー）を開く処理
        if (Input.GetKeyDown(KeyCode.Tab) && startAnimTime_ >= 7.0f)
        {
            pauseFlag_ = !pauseFlag_;
        }

        ActiveText();

        // 時間計測毎フレーム呼ばれないようにフラグで管理
        if (secondsAddFlag_ == true)
        {
            if (seconds >= 60)
            {
                // count_60以上＝1分
                minute++;
                seconds = seconds - 60;
            }
            StartCoroutine("Coroutine");
            secondsAddFlag_ = false;
        }
        timerText.text = minute.ToString("00") + ":" + (seconds).ToString("00");
    }

    private void ActiveText()
    {
        if (SceneManager.GetActiveScene().name == "TutorialScene")
        {
            // チュートリアル中は電池の消費なしのため
            batteryText.text = "電池残量100%";
            // チュートリアル中は基礎行動で1つ、実践で1つ
            keyText[0].text = "(" + playerColScript.GetkeyItemCnt() + "/ 1)";
            keyText[1].text = "×\n" + playerColScript.GetkeyItemCnt();
        }
        else
        {
            // バッテリーの残り
            batteryText.text = "電池残量" + batteryScript.ReturnBatteryRest() + "%";
            // 鍵の所持数表示    // [0]ポーズ画面で　[1]ゲーム中
            keyText[0].text = "(" + playerColScript.GetkeyItemCnt() + "/ 8)";
            keyText[1].text = "×\n" + playerColScript.GetkeyItemCnt();
        }
    }

    public bool GetPauseFlag()
    {
        return pauseFlag_;
    }
    public float GetStartAnimTime()
    {
        return startAnimTime_;
    }

    private IEnumerator Coroutine()
    {
        //１秒待機
        // WaitForSecondsRealtimeにすることでTimeScaleの影響を受けない
        yield return new WaitForSecondsRealtime(1.0f);
        seconds++;        // 1秒経過したらcount_が+1される
        secondsAddFlag_ = true;
    }
}
