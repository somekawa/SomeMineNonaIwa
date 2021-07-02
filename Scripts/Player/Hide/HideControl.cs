using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControl : MonoBehaviour
{
    private bool hideFlg_ = false;              // 箱に隠れているか

    private GameObject mainCamera_;             // player側のカメラ

    private GameObject boxCamera_;              // 箱の中のカメラ
    private GameObject boxLamp_;                // 箱の中のランプ

    private float stayTime_;                    // 箱の中に隠れた時間
    private float timeMin_        = 1.0f;       // 箱の中に隠れる最小時間
    private float releaseTime_;                 // 箱から出た時間
    private const float coolTime_ = 1.0f;       // 次に箱の中に入れるまでの時間

    // Start is called before the first frame update
    void Start()
    {
        mainCamera_ = gameObject.transform.Find("Main Camera").gameObject;
        releaseTime_ = Time.time - coolTime_;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hideFlg_)
        {
            // 箱から出ている
            return;
        }

        if ((Input.GetKey(KeyCode.F)) && (Time.time - stayTime_ >= timeMin_)) 
        {
            // 箱から出る処理
            if (IsBoxCamera())
            {
                // カメラを箱の中からPlayer側に切り替える
                boxCamera_.gameObject.SetActive(false);
                mainCamera_.gameObject.SetActive(true);
                boxCamera_ = null;
            }
            if (IsBoxLamp())
            {
                // ランプを消す
                boxLamp_.gameObject.SetActive(false);
                boxLamp_ = null;
            }

            stayTime_ = 0.0f;
            releaseTime_ = Time.time;
            hideFlg_ = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (hideFlg_ || (Time.time - releaseTime_ <= coolTime_)) 
        {
            // 箱の中に隠れている、またはクールタイムが終わっていない
            return;
        }

        if (other.gameObject.tag == "HideObj")
        {
            if (!Input.GetKey(KeyCode.F))
            {
                return;
            }

            // 隠れる処理
            boxCamera_= other.gameObject.transform.Find("BoxCamera").gameObject;
            if (IsBoxCamera()) 
            {
                // カメラをPlayer側から箱の中に切り替える
                mainCamera_.gameObject.SetActive(false);
                boxCamera_.gameObject.SetActive(true);
            }

            boxLamp_ = other.gameObject.transform.Find("BoxLight").gameObject;
            if(IsBoxLamp())
            {
                // ランプをつける
                boxLamp_.gameObject.SetActive(true);
            }

            hideFlg_ = true;
            stayTime_ = Time.time;
            releaseTime_ = 0.0f;
        }
    }

    public bool GetHideFlg()
    {
        return hideFlg_;
    }

    private bool IsBoxCamera()
    {
        if (boxCamera_ != null)
        {
            return true;
        }
        return false;
    }

    private bool IsBoxLamp()
    {
        if (boxLamp_ != null)
        {
            return true;
        }
        return false;
    }
}
