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

    private bool pauseFlag_;

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

    void Update()
    {
        // ポーズ（メニュー）を開く処理
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            pauseFlag_ = !pauseFlag_;
        }

        ActiveText();

        // 時間計測毎フレーム呼ばれないようにフラグで管理
        if (secondsAddFlag_ == true)
        {
            if (seconds >= 60f)
            {
                // count_60以上＝1分
                minute++;
                seconds = seconds - 60;
            }
            StartCoroutine("Coroutine");
            secondsAddFlag_ = false;
        }
        timerText.text = minute.ToString("00") + ":" + (seconds).ToString("00");
        // Debug.Log("Coroutine"+ seconds);

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

    private IEnumerator Coroutine()
    {
        //処理１
       // Debug.Log("Start");

        //１秒待機
        // WaitForSecondsRealtimeにすることでTimeScaleの影響を受けない
        yield return new WaitForSecondsRealtime(1.0f);
        seconds++;        // 1秒経過したらcount_が+1される
        secondsAddFlag_ = true;
    }

}
