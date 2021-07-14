using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControl : MonoBehaviour
{
    private bool hideFlg_ = false;              // 箱に隠れているか

    private GameObject mainCamera_;             // player側のカメラ

    private GameObject lastInBox_;
    private GameObject boxCamera_;              // 箱の中のカメラ
    private GameObject boxLamp_;                // 箱の中のランプ
    private HideBox hideBox_;

    private float stayTime_;                    // 箱の中に隠れた時間
    private float timeMin_        = 1.0f;       // 箱の中に隠れる最小時間
    private float releaseTime_;                 // 箱から出た時間
    private const float coolTime_ = 1.0f;       // 次に箱の中に入れるまでの時間

    // リザルト画面に出すための隠れた回数
    public static int hideNum;
    
    void Start()
    {
        mainCamera_ = gameObject.transform.Find("Main Camera").gameObject;
        releaseTime_ = Time.time - coolTime_;
        hideNum = 0;
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
                boxCamera_.SetActive(false);
                mainCamera_.SetActive(true);
                boxCamera_ = null;
            }
            if (IsBoxLamp())
            {
                // ランプを消す
                boxLamp_.SetActive(false);
                boxLamp_ = null;
            }
            if (IsHideBox())
            {
                hideBox_.SetInFlag(false);
                hideBox_ = null;
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
            if (!Input.GetKey(KeyCode.F)||
                (other.gameObject.GetComponent<HideBox>().GetMannequin()))
            {
                return;
            }

            if ((hideBox_) && (lastInBox_ != other.gameObject))
            {
                // 前回まで入っていた箱と違っている
                hideBox_.SetLastInFlag(false);
            }
            lastInBox_ = other.gameObject;

            // 隠れる処理
            boxCamera_ = lastInBox_.transform.Find("BoxCamera").gameObject;
            if (IsBoxCamera()) 
            {
                // カメラをPlayer側から箱の中に切り替える
                mainCamera_.SetActive(false);
                boxCamera_.SetActive(true);

                hideNum++;
                Debug.Log("hideNum" + hideNum);
            }

            boxLamp_ = lastInBox_.transform.Find("BoxLight").gameObject;
            if(IsBoxLamp())
            {
                // ランプをつける
                boxLamp_.SetActive(true);
            }               

            hideBox_ = lastInBox_.GetComponent<HideBox>();
            if (IsHideBox())
            {
                hideBox_.SetInFlag(true);
                hideBox_.SetLastInFlag(true);
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

    private bool IsHideBox()
    {
        if (hideBox_ != null)
        {
            return true;
        }
        return false;
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
