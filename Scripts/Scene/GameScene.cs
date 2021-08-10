using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameScene : MonoBehaviour
{
    public PlayerCollision playerColScript;
    public tBatteryScript batteryScript;
    public Text pauseKeyText;   // ポーズ中に表示される鍵の数
    public Text mainKeyText;    // ゲーム再生中に表示される鍵の数
    public Text batteryText;    // ポーズで電池残量の数値を表示

    private bool pauseFlag_;    // ポーズ中かどうか
    // クリアシーンで使うため分と秒はpublicに
    public static int minute  = 0;          // 何分か
    public static int seconds = 0;          // 何秒か
    public Text timerText;                  // タイマー表示用テキスト
    private bool secondsAddFlag_ = false;   // 秒数が足されたか

    public GameObject collectCanvas;
    private GameObject[] collectUIs_;


    // 開始時のPlayerのアニメーション関係
    private float startAnimTime_ = 0.0f;
    private const float maxAnimTime_ = 10.0f;   // 再生時間

    void Start()
    {
        StartCoroutine("Coroutine");
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            collectUIs_ = collectCanvas.gameObject.GetComponentsInChildren<Transform>().Select(t => t.gameObject).ToArray();
            for (int i = 1; i < collectUIs_.Length - 2; i++)
            {
                collectUIs_[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        // スタート時のアニメーション中はTABキー操作ができないようにする
        if (startAnimTime_ < GetMaxAnimTime())
        {
            startAnimTime_ += Time.deltaTime;
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "TutorialScene")
            {

                for (int i = 1; i < collectUIs_.Length - 2; i++)
                {
                    collectUIs_[i].gameObject.SetActive(false);
                }
            }
            // ポーズ（メニュー）を開く処理
            if (Input.GetKeyDown(KeyCode.Tab) && startAnimTime_ >= GetMaxAnimTime())
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
            pauseKeyText.text = "(" + playerColScript.GetkeyItemCnt() + "/ 1)";
            mainKeyText.text = "×\n" + playerColScript.GetkeyItemCnt();
        }
        else
        {
            // バッテリーの残り
            batteryText.text = "電池残量" + (int)batteryScript.ReturnBatteryRest() + "%";
            // 鍵の所持数表示
            pauseKeyText.text = "(" + playerColScript.GetkeyItemCnt() + "/ 8)";
            mainKeyText.text = "×\n" + playerColScript.GetkeyItemCnt();
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
        if (SceneManager.GetActiveScene().name != "MainScene") 
        {
            // ゲーム画面以外ではアニメーションは再生しない
            return 0.0f;
        }
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
