using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class effectScript : MonoBehaviour
{
    public AudioClip walkSound;             // プレイヤーの足音SE

    private float count_ = 0.0f;            // エフェクト発生時間の管理用
    private bool countFlg_  = false;        // カウント最大時にtrue
    private bool effectPlayFlg_ = false;    // エフェクト再生中にtrue
    private float playTime_ = 0.0f;         // エフェクト再生時間の管理用
    private GameObject effect_ = null;      // エフェクト情報
    private GameObject player_ = null;      // プレイヤー情報
    private playerController plSc_ = null;  // プレイヤーの操作情報を取得する際に利用

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "TutorialScene")
        {
            // プレイヤー情報を取得する
            player_ = GameObject.Find("tPlayer");
            if (player_ != null)
            {
                plSc_ = player_.GetComponent<playerController>();
            }
            else
            {
                Debug.Log("Playerがnullでエラー");
            }
        }
        else
        {
            // プレイヤー情報を取得する
            player_ = GameObject.Find("Player");
            if (player_ != null)
            {
                plSc_ = player_.GetComponent<playerController>();
            }
            else
            {
                Debug.Log("Playerがnullでエラー");
            }
        }


        if (plSc_ == null)
        {
            Debug.Log("plScがnullでエラー");
        }
    }

    void Update()
    {
        // plSc_にアクセスする回数が多かった為、Updateの最初に一度取得するようにして処理を軽くする
        playerController tmpPlSc = plSc_;

        // 指定秒数でフラグを立てる
        count_ += Time.deltaTime;

        if ((count_ >= tmpPlSc.GetCountMax()) && !countFlg_)
        {
            countFlg_ = true;
            effectPlayFlg_ = true;
        }

        // フラグがtrueの時、エフェクトを再生させる
        if (countFlg_ && effectPlayFlg_)
        {
            if (effect_ == null)
            {
                if(tmpPlSc.GetSlowWalkFlg())
                {
                    // 低速歩行
                    effect_ = GameObject.Find("FootEffect");
                }
                else
                {
                    // 通常歩行
                    effect_ = GameObject.Find("FootEffect_Y");
                }
                if (effect_ == null)
                {
                    Debug.Log("FootEffect関連がnullでエラー");
                }
            }

            // プレイヤーが移動しているときだけ再生するように設定
            if (tmpPlSc.GetWalkFlg())
            {
                effect_.GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().clip = walkSound;
                GetComponent<AudioSource>().Play();
            }

            count_ = 0;
            countFlg_ = false;
        }

        if (effectPlayFlg_ && (playTime_ < 0.5f))
        {
            playTime_ += Time.deltaTime;
            return; // エフェクト再生中かつ、再生時間が一定値以下の場合はreturn
        }

        // エフェクトの再生を停止する
        if (effect_ != null)
        {
            effect_.GetComponent<ParticleSystem>().Stop();
            playTime_ = 0.0f;
            effect_ = null;
            effectPlayFlg_ = false;
        }
    }
}