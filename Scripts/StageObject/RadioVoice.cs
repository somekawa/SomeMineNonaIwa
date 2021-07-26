using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RadioVoice : MonoBehaviour
{
    public AudioClip VoiceSound;
    private AudioSource source_;

    private bool soundFlg_ = false;
    private float voiceTimeMax = 20.0f;
    private float nowVoiceTime = 0.0f;

    private float radioOnOffTime_ = 0.0f;

    // Sliderのワープ関連変数
    //private GameObject[] slenderMan_;
    private SlenderManCtl[] slenderManCtl_;
    //private float minDistance_;
    //private float nowDistance_;
    //private int minCnt_;

    void Start()
    {
        // AudioSourceの情報を最初に取得しておく
        source_ = GetComponent<AudioSource>();
        //slenderMan_ = new GameObject[4];
        slenderManCtl_ = new SlenderManCtl[4];
    }

    void Update()
    {
        if (radioOnOffTime_ > 0.0f)
        {
            // ラジオ操作を受け付けない
            radioOnOffTime_ -= Time.deltaTime;
        }

        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().spawnSlender[i] != null)
            {
                slenderManCtl_[i] = SlenderSpawner.GetInstance().spawnSlender[i].gameObject.GetComponent<SlenderManCtl>();
            }
        }

        if (!soundFlg_)
        {
            slenderManCtl_[SlenderSpawner.GetInstance().GetMinCnt()].ringingFlag = false;
            return;
        }
        else
        {
            SlenderSpawner.GetInstance().ClosestObject(this.gameObject, 2, true, false);
            //for (int x = 0; x < SlenderSpawner.GetInstance().spawnSlender.Length; x++)
            //{
            //    if (slenderMan_[x] != null)
            //    {
            //        nowDistance_ = Vector3.Distance(gameObject.transform.position, slenderMan_[x].transform.position);
            //        if (minDistance_ >= nowDistance_)
            //        {
            //            minDistance_ = nowDistance_;
            //            minCnt_ = x;
            //        }
            //    }
            //}
            //if (slenderManCtl_ != null)
            //{
            //    slenderManCtl_[minCnt_].soundPoint.x = this.gameObject.transform.position.x;
            //    slenderManCtl_[minCnt_].soundPoint.z = this.gameObject.transform.position.z;
            //    slenderManCtl_[minCnt_].navMeshAgent_.stoppingDistance = 2;
            //    slenderManCtl_[minCnt_].listenFlag = true;
            //    slenderManCtl_[minCnt_].ringingFlag = true;
            //}
        }

        if (nowVoiceTime >= voiceTimeMax)
        {
            CommonSoundStop();
            //slenderManCtl_[minCnt_].listenFlag = false;
        }
        else
        {
            nowVoiceTime += Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (radioOnOffTime_ > 0.0f)
        {
            return;
        }

        if (other.gameObject.tag == "ItemHitArea" && Input.GetKeyUp(KeyCode.E))
        {
            if (!soundFlg_)   // SEのON
            {
                if (VoiceSound != null)
                {
                    source_.clip = VoiceSound;
                    source_.Play();
                    soundFlg_ = true;
                    //Debug.Log("SEのON");
                    radioOnOffTime_ = 1.0f;
                    return;
                }
            }
            else              // SEのOFF
            {
                CommonSoundStop();
                //Debug.Log("SEのOFF");
                radioOnOffTime_ = 1.0f;
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
}