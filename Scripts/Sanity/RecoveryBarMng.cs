using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryBarMng : MonoBehaviour
{
    private Image rBarImage_;               // 耐久時間用バー
    private Vector2 rBarMaxSize_;           // 耐久時間用バーの最大サイズ
    private float rBarActiveTime_ = 0.0f;   // 耐久時間用バーが変動なく表示されている時間  
    private Color color_;

    // Start is called before the first frame update
    void Start()
    {
        rBarImage_ = gameObject.transform.Find("SanitCanvas/RecoveryBarImage").gameObject.GetComponent<Image>();
        rBarMaxSize_ = rBarImage_.rectTransform.sizeDelta;
        color_ = rBarImage_.color;
    }

    // Update is called once per frame
    void Update()
    {
        float d_timeMax = gameObject.GetComponent<SanitMng>().GetDTimeMax();
        float d_nowTime = gameObject.GetComponent<SanitMng>().GetDNTimeMax();
        float rBarX = (rBarMaxSize_.x / d_timeMax) * d_nowTime;

        if (rBarX < rBarMaxSize_.x) 
        {
            color_.a = 0.2f;           
            rBarActiveTime_ = 0.0f;
        }

        if (d_nowTime <= 3.0f)
        {
            // 赤くなる
            color_ = new Color(1.0f, 0.0f, 0.0f, color_.a);
        }
        else if(rBarX == rBarMaxSize_.x)
        {
            // 耐久時間が最大になったら緑になる
            color_ = new Color(0.0f, 1.0f, 0.0f, color_.a);
        }
        else
        {
            // 白に戻る
            color_ = new Color(1.0f, 1.0f, 1.0f, color_.a);
        }

        if (d_nowTime == 0.0f)
        {
            rBarX = 0.0f;
        }
        rBarImage_.rectTransform.sizeDelta = new Vector2(rBarX, rBarMaxSize_.y);

        if (rBarActiveTime_ == 0.0f)
        {
            if (rBarX == rBarMaxSize_.x)
            {
                rBarActiveTime_ = Time.time;
            }
        }
        else
        {
            if (Time.time - rBarActiveTime_ >= 3.0f)
            {
                color_.a -= 0.01f;
                if (color_.a <= 0.0f)
                {
                    color_.a = 0.0f;
                }
            }
        }

        rBarImage_.color = color_;
    }
}
