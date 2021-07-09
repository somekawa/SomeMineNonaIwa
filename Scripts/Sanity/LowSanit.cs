using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LowSanit : MonoBehaviour
{
    public Image[] bloodImage;      // ボックスにつける血の画像　2枚
    private float speed_;           // 点滅スピード
    private float alphaTime_;       // 文字のアルファ値を変えて点滅

    private float erosionPoint_;    // 侵蝕度＝fillAmoune
    private float getSanit_;        // 正気度を取得

    public NoiseControl NoiseCtl;   // 血のノイズを参照するため

    void Start()
    {
        // 正気度を表示 
        getSanit_ = (int)SanitMng.sanit_;
        speed_ = 0.1f;

        erosionPoint_ = 0;

        /*画像１*/
        bloodImage[0].fillAmount = erosionPoint_;
        bloodImage[0].enabled = false;
        /*画像２*/
        bloodImage[1].fillAmount = erosionPoint_;
        bloodImage[1].enabled = false;
        getSanit_ = SanitMng.sanit_;
    }

    void Update()
    {
        // Sanitを更新させる
        getSanit_ = SanitMng.sanit_;
        Debug.Log("残り正気度" + getSanit_);

        if (getSanit_ <= 70&&erosionPoint_<1.0f)
        {
            //fillamountが最大になったら入らないようにする
            bloodImage[0].enabled = true;
            bloodImage[1].enabled = true;
            erosionPoint_ += 0.002f;

        }

        if (NoiseCtl.GetBRawImage()==true)
        {
            // 侵蝕完了　正気度２０の時　（赤いノイズ）
            Debug.Log("血のノイズに変更されました");
            erosionPoint_ = 1.0f;
            bloodImage[0].color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
            bloodImage[1].color = new Color(0.0f, 0.0f, 0.0f, 255.0f);
        }
        else
        {
            // 点滅中
            bloodImage[0].color = GetAlphaColor(bloodImage[0].color);
            bloodImage[1].color = GetAlphaColor(bloodImage[1].color);
        }
        bloodImage[0].fillAmount = erosionPoint_;
        bloodImage[1].fillAmount = erosionPoint_;

    }

    Color GetAlphaColor(Color color)
    {
        // 文字の点滅
        alphaTime_ += Time.deltaTime * 5.0f * speed_;
        color.a = Mathf.Sin(alphaTime_) * 0.5f + 1.0f;
       // Debug.Log("alphaTime_" + alphaTime_);
        return color;
    }

}
