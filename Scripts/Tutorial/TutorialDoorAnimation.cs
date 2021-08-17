using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDoorAnimation : MonoBehaviour
{
    public GameObject door;            // 回転するオブジェクト格納用
    private bool fullOpenFlag_ = false; // 扉が開ききったらtrue
    private float maxAngle_ = 90.0f;    // 引き扉：回転した後の角度
   
    public Image fadePanel;
    private float panelAlpha_ = 0.0f;

    void Start()
    {
        
    }

    void Update()
    {      
        // ステージチェンジし終わったら
        if (fullOpenFlag_ == true)
        {
            fadePanel.color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
            fadePanel.enabled = false;// 画像を消す
            Debug.Log("フェード画像を透過");
        }
        else
        {
            if (door.transform.childCount <= 0)
            {
                // SEの音を鳴らす(ドアを開く音)
                SoundScript.GetInstance().PlaySound(12);

                Debug.Log("のドアに接触しました");
                float step = 120.0f * Time.deltaTime;
                panelAlpha_ += 0.01f;

                // 指定した方向にゆっくり回転する場合
                door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0.0f, maxAngle_, 0.0f), step);

                if (1.0f < panelAlpha_)
                {
                    //openFlag_ = false;
                    fullOpenFlag_ = true;
                }
                fadePanel.color = new Color(255.0f, 255.0f, 255.0f, panelAlpha_);

            }
        }

        //if (openFlag_ == true)
        //{
        //    // SEの音を鳴らす(ドアを開く音)
        //    SoundScript.GetInstance().PlaySound(12);

        //    Debug.Log("のドアに接触しました");
        //    float step = 120.0f * Time.deltaTime;
        //    panelAlpha_ += 0.01f;

        //    // 指定した方向にゆっくり回転する場合
        //    door.transform.rotation = Quaternion.RotateTowards(door.transform.rotation, Quaternion.Euler(0.0f, -maxAngle_, 0.0f), step);

        //    if (1.0f < panelAlpha_)
        //    {
        //        openFlag_ = false;
        //        fullOpenFlag_ = true;
        //    }
        //    fadePanel.color = new Color(255.0f, 255.0f, 255.0f, panelAlpha_);
        //}

    }

    public bool GetOpenFlag()
    {
        return fullOpenFlag_;
    }
}
