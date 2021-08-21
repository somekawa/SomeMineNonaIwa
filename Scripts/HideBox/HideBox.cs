using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideBox : MonoBehaviour
{
    private GameObject lid_;            // 蓋

    private Outline outline_;

    // マネキン
    public bool mannequinFlag_;
    private GameObject mannequin_;

    private bool hideNowFlag_ = false;  // 入ってる
    private bool lastInFlag_  = false;  // 最後に入った箱なのか
    private float inTime_     = 0.0f;   // 連続で入った時間

    // Start is called before the first frame update
    void Start()
    {
        outline_ = gameObject.GetComponent<Outline>();

        mannequin_= transform.Find("Mannequin").gameObject;
        mannequin_.SetActive(mannequinFlag_);

        lid_ = transform.Find("Crate03b").gameObject;
        lid_.SetActive(!mannequinFlag_);
    }

    // Update is called once per frame
    void Update()
    {

        if (mannequinFlag_)
        {
            // マネキンが中に入っている
            inTime_ = 0.0f;
            outline_.enabled = false;
            return;
        }

        if (!hideNowFlag_) 
        {
            // 箱に入ってない
            if (!lastInFlag_)
            {
                // 他の箱に入った
                inTime_ = 0.0f;
            }
            return;
        }

        // 箱に入っている
        outline_.enabled = false;
        inTime_ += Time.deltaTime;
    }

    // 今入れる状況か確認
    public bool InFlagCheck()
    {
        if((mannequinFlag_) ||  // マネキンが入っている
            (hideNowFlag_)||    // 今入っている
            (!outline_.enabled)) // プレイヤーが見つけていない
        {
            return false;
        }

        return true;
    }

    public void SettestMannequin(bool flag)
    {
        if (mannequin_ == null)
        {
            Debug.Log("マネキンが見つかりません");
            Start();
        }
        mannequinFlag_ = flag;

        mannequin_.SetActive(mannequinFlag_);
        lid_.SetActive(!mannequinFlag_);
    }

    public void SetInFlag(bool flag)
    {
        hideNowFlag_ = flag;
    }

    public void SetLastInFlag(bool flag)
    {
        lastInFlag_ = flag;
    }

   public float GetInTime()
    {
        return inTime_;
    }
}
