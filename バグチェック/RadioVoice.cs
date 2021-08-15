using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name != "TutorialScene")
        {
            for (int i = 0; i < SlenderSpawner.GetInstance().spawnSlender.Length; i++)
            {
                if (slenderManCtl_[i] == null && SlenderSpawner.GetInstance().slenderManCtl[i] != null)
                {
                    slenderManCtl_[i] = SlenderSpawner.GetInstance().slenderManCtl[i];
                }
            }

            if (!soundFlg_)
            {
                if (slenderManCtl_[SlenderSpawner.GetInstance().GetMinCnt()] != null)
                {
                    slenderManCtl_[SlenderSpawner.GetInstance().GetMinCnt()].ringingFlag = false;
                }
                return;
            }
            else
            {
                SlenderSpawner.GetInstance().ClosestObject(this.gameObject, 2, true, false);
            }
        }
        else
        {
            if (!soundFlg_)
            {
                return;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ItemHitArea" && Input.GetKeyUp(KeyCode.E))
        {
            // RadioVoiceAudioのSounds関数を呼ぶ
            voiceAudio_.Sounds(int.Parse(this.gameObject.name), this.gameObject.transform.position);
        }

        if (other.gameObject.tag == "ItemHitArea")
        {
            Debug.Log("アイテムヒットエリアと接触中");
            voiceAudio_.SetVoiceAround(true);
            voiceAudio_.SetRadioNum(int.Parse(this.gameObject.name));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // ItemHitAreaの範囲外になったらfalse 
        if (other.gameObject.tag == "ItemHitArea")
        {
            voiceAudio_.SetVoiceAround(false);
        }
    }

}