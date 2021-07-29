using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioVoice : MonoBehaviour
{
    public GameObject voiceAudio;
    private RadioVoiceAudio voiceAudio_;
    private AudioSource source_;

    private bool soundFlg_ = false;

    // Sliderのワープ関連変数
    private SlenderManCtl[] slenderManCtl_;

    void Start()
    {
        voiceAudio_ = voiceAudio.GetComponent<RadioVoiceAudio>();
        slenderManCtl_ = new SlenderManCtl[4];
    }

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
            slenderManCtl_[SlenderSpawner.GetInstance().GetMinCnt()].ringingFlag = false;
            return;
        }
        else
        {
            SlenderSpawner.GetInstance().ClosestObject(this.gameObject, 2, true, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea" && Input.GetKeyUp(KeyCode.E))
        {
            // RadioVoiceAudioのSounds関数を呼ぶ
            voiceAudio_.Sounds(int.Parse(this.gameObject.name), this.gameObject.transform.position);
        }
    }
}