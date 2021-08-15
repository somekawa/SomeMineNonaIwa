using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ラジオを1つ鳴らしている間は、他のラジオを再生できないようにしています

public class RadioVoiceAudio : MonoBehaviour
{
    public AudioClip VoiceSound;

    private AudioSource source_;
    private float radioOnOffTime_ = 0.0f;
    private bool soundFlg_ = false;
    private float voiceTimeMax = 20.0f;
    private float nowVoiceTime = 0.0f;

    private int radioNum_ = -1;

    public Image monochromeUI;     // 再生中のUI
    private float eraseCnt_ = 0.0f;// 再生時間に対するfillAmountが1秒に減る値
    private bool nowVoice_ = false;     // UI用　音が鳴っている間に表示を変える
    private bool playVoice_ = false;    // メッセージ用　音が鳴っているか
    private bool nowRoundFlag_ = false; // BGM再生オブジェクトの範囲にいるかどうか
    private int nowTime = 1;            // nowVoiceが加算されるごとに1増える 1からなのはUIを表示する時間必要なため

    void Start()
    {
        // AudioSourceの情報を最初に取得しておく
        source_ = GetComponent<AudioSource>();

        eraseCnt_ = 1.0f / (int)voiceTimeMax;
        Debug.Log("eraseCnt_" + eraseCnt_);
    }

    void Update()
    {
        if (radioOnOffTime_ > 0.0f)
        {
            // ラジオ操作を受け付けない
            radioOnOffTime_ -= Time.deltaTime;
        }

        if (!soundFlg_)
        {
            return;
        }

        if (soundFlg_ == true && nowTime < 2)
        {
            // 1秒の間に徐々に使用不可状態UIを表示する
            monochromeUI.fillAmount += 0.005f;
        }
        if (nowVoiceTime >= voiceTimeMax)
        {
            CommonSoundStop();
        }
        else
        {
            nowVoiceTime += Time.deltaTime;
          nowVoice_ = true;       // 使用中のためメッセージが出ないようにする 

            if (nowTime < (int)nowVoiceTime)
            {
                // 1秒ごとに
                nowTime += 1;
                monochromeUI.fillAmount -= eraseCnt_;
            }
        }
        Debug.Log("Updateラジオの番号" + radioNum_);

    }

    public void Sounds(int radioNum,Vector3 pos)
    {
        if (radioOnOffTime_ > 0.0f)
        {
            return;
        }

        if (!soundFlg_)   // SEのON
        {
            if (VoiceSound != null)
            {
                source_.clip = VoiceSound;
                source_.Play();
                soundFlg_ = true;
                Debug.Log("SEのON");
                radioOnOffTime_ = 1.0f;

                // ラジオ番号の受け取り
                radioNum_ = radioNum;
                // VoiceAudioのオブジェクトを再生中のラジオの場所に移動させる
                this.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
                return;
            }
        }
        else              // SEのOFF
        {
            // 再生中の同じラジオじゃないと停止できないようにする
            if (radioNum == radioNum_)
            {
                CommonSoundStop();
                Debug.Log("SEのOFF");
                radioOnOffTime_ = 1.0f;
            }
            else
            {
                Debug.Log("別のラジオの為、OFFにできない");
            }
        }
    }

    // 音再生の共通ストップ処理
    void CommonSoundStop()
    {
        source_.Stop();
        soundFlg_ = false;
        nowVoiceTime = 0.0f;

        nowVoice_ = false;
        monochromeUI.fillAmount = 0.0f;
        nowTime = 1;
    }

    public void SetVoiceAround(bool flag)
    {
        playVoice_ = flag;
    }

    public bool GetSoundFlag()
    {
        return soundFlg_;
    }

    public bool GetRadioAround()
    {
        return playVoice_;
    }

    public bool GetNowVoice()
    {
        return nowVoice_;
    }

    public bool GetNowRound()
    {
        return nowRoundFlag_;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea")
        {
            nowRoundFlag_ = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        nowRoundFlag_ = false;
    }

}
