using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectScript : MonoBehaviour
{
    [SerializeField]
    private int count = 0;
    private bool countFlg = false;
    private bool onceFlg  = false;
    private float playTime = 0.0f;
    private GameObject effect = null;

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
        count++;
        if (count >= 120 && !countFlg)
        {
            countFlg = true;
            onceFlg = true;
        }

        // フラグがtrueの時、エフェクトを再生させる
        if (countFlg)
        {
            if (onceFlg)
            {
                if (effect == null)
                {
                    effect = GameObject.Find("FootEffect");
                    if (effect == null)
                    {
                        Debug.Log("FootEffectがnullでエラー");
                    }
                }

                // プレイヤーが移動しているときだけ再生するように設定
                if (plSc.GetWalkFlg())
                {
                    effect.GetComponent<ParticleSystem>().Play();
                }

                count = 0;
                countFlg = false;
            }
        }

        if (onceFlg)
        {
            playTime += Time.deltaTime;
        }

        if (playTime >= 0.5f)
        {
            if (effect != null)
            {
                effect.GetComponent<ParticleSystem>().Stop();
                playTime = 0.0f;
            }
            effect = null;
            onceFlg = false;
        }
    }
}