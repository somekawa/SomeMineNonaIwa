using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        // AudioSourceの情報を最初に取得しておく
        source_ = GetComponent<AudioSource>();
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

        if (nowVoiceTime >= voiceTimeMax)
        {
            CommonSoundStop();
        }
        else
        {
            nowVoiceTime += Time.deltaTime;
        }

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
            if(radioNum == radioNum_)
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
    }

    public bool GetSoundFlg()
    {
        return soundFlg_;
    }
}
