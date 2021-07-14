using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioVoice : MonoBehaviour
{
    public AudioClip VoiceSound;
    private bool soundFlg_ = false;
    private float voiceTimeMax = 20.0f;
    private float nowVoiceTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!soundFlg_)
        {
            return;
        }

        if(nowVoiceTime >= voiceTimeMax)
        {
            GetComponent<AudioSource>().Stop();
            soundFlg_ = false;
            nowVoiceTime = 0.0f;
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
