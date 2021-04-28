using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectScript : MonoBehaviour
{
    [SerializeField]
    private int count_ = 0;
    private bool countFlg_  = false;
    private bool onceFlg_   = false;
    private float playTime_ = 0.0f;
    private GameObject effect_ = null;

    private GameObject player;
    private playerController plSc;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        if (player != null)
        {
            plSc = player.GetComponent<playerController>();
        }
        else
        {
            Debug.Log("Playerがnullでエラー");
        }

        if (plSc == null)
        {
            Debug.Log("plScがnullでエラー");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 120秒でフラグを立てる
        count_++;
        if (count_ >= 120 && !countFlg_)
        {
            countFlg_ = true;
            onceFlg_ = true;
        }

        // フラグがtrueの時、エフェクトを再生させる
        if (countFlg_)
        {
            if (onceFlg_)
            {
                if (effect_ == null)
                {
                    if(plSc.GetSlowWalkFlg())
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
                if (plSc.GetWalkFlg())
                {
                    effect_.GetComponent<ParticleSystem>().Play();
                }

                count_ = 0;
                countFlg_ = false;
            }
        }

        if (onceFlg_)
        {
            playTime_ += Time.deltaTime;
        }

        if (playTime_ >= 0.5f)
        {
            if (effect_ != null)
            {
                effect_.GetComponent<ParticleSystem>().Stop();
                playTime_ = 0.0f;
            }
            effect_ = null;
            onceFlg_ = false;
        }
    }
}