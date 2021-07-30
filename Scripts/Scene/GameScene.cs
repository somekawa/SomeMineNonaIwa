using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameScene : MonoBehaviour
{
    // Component取得用変数
    public tBatteryScript batteryScript;
    public PlayerCollision playerColScript;

    public Text batteryText;
    public Text[] keyText;
    public GameObject collectCanvas;

    private bool pauseFlag_;
    private GameObject[] collectUIs_;

    // クリアシーンで使うため分と秒はpublicに
    public static int minute  = 0;          // 何分か
    public static int seconds = 0;          // 何秒か
    public Text timerText;                  // タイマー表示用テキスト
    private bool secondsAddFlag_ = false;   // 秒数が足されたか

    // 開始時のPlayerのアニメーション関係
    private float startAnimTime_ = 0.0f;
    private const float maxAnimTime_ = 10.0f;   // 再生時間

    void Start()
    {
        StartCoroutine("Coroutine");
        collectUIs_ = collectCanvas.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
        for (int i = 1; i < collectUIs_.Length - 2; i++)
        {
            collectUIs_[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // スタート時のアニメーション中はTABキー操作ができないようにする
        if (startAnimTime_ < maxAnimTime_)
        {
            startAnimTime_ += Time.deltaTime;
        }
        else
        {
            for (int i = 1; i < collectUIs_.Length - 2; i++)
            {
                collectUIs_[i].gameObject.SetActive(true);
            }

            // ポーズ（メニュー）を開く処理
            if (Input.GetKeyDown(KeyCode.Tab) && startAnimTime_ >= maxAnimTime_)
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

    public float GetMaxAnimTime()
    {
        return maxAnimTime_;
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
