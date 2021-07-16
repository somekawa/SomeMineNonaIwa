using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioVoice : MonoBehaviour
{
    public AudioClip VoiceSound;
    private bool soundFlg_ = false;
    private float voiceTimeMax = 20.0f;
    private float nowVoiceTime = 0.0f;

    // Sliderのワープ関連変数
    private GameObject[] slenderMan_;
    private SlenderManCtl[] slenderManCtl_;
    private float minDistance_;
    private float nowDistance_;
    private int minCnt_;

    // Start is called before the first frame update
    void Start()
    {
        slenderMan_ = new GameObject[4];
        slenderManCtl_ = new SlenderManCtl[4];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
        {
            if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().spawnSlender[i] != null)
            {
                slenderManCtl_[i] = SlenderSpawner.GetInstance().spawnSlender[i].gameObject.GetComponent<SlenderManCtl>();
            }
        }
        if (!soundFlg_)
        {
            return;
        }
        else
        {
            for (int x = 0; x < SlenderSpawner.GetInstance().spawnSlender.Length; x++)
            {
                if (slenderMan_[x] != null)
                {
                    nowDistance_ = Vector3.Distance(gameObject.transform.position, slenderMan_[x].transform.position);
                    if (minDistance_ >= nowDistance_)
                    {
                        minDistance_ = nowDistance_;
                        minCnt_ = x;
                    }
                }
            }
                if (slenderManCtl_ != null)
                {
                    slenderManCtl_[minCnt_].soundPoint.x = this.gameObject.transform.position.x;
                    slenderManCtl_[minCnt_].soundPoint.z = this.gameObject.transform.position.z;
                    slenderManCtl_[minCnt_].listenFlag = true;
                }
        }

        if (nowVoiceTime >= voiceTimeMax)
        {
            GetComponent<AudioSource>().Stop();
            soundFlg_ = false;
            nowVoiceTime = 0.0f;
            slenderManCtl_[minCnt_].listenFlag = false;
        }
        else
        {
            nowVoiceTime += Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea" && Input.GetKeyUp(KeyCode.E))
        {
            if (VoiceSound != null)
            {
                GetComponent<AudioSource>().clip = VoiceSound;
                GetComponent<AudioSource>().Play();
                soundFlg_ = true;
            }
        }
    }
}
