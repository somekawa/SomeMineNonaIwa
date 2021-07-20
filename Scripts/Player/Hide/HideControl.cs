using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideControl : MonoBehaviour
{
    private bool hideFlg_ = false;              // 箱に隠れているか

    private GameObject mainCamera_;             // player側のカメラ

    private GameObject lastInBoxObj_;
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

        // 箱に隠れる処理
        if ((Input.GetKey(KeyCode.E)) && (Time.time - stayTime_ >= timeMin_)) 
        {
            // 箱から出る処理
            ChangeHideAction(false);
            // 取得していたオブジェクトを削除
            // hideBox_は次箱に入るときに必要なので残す
            boxCamera_ = null;
            boxLamp_ = null;

            stayTime_ = 0.0f;
            releaseTime_ = Time.time;
            hideFlg_ = false;
        }
    }

    public void HideBoxAction(GameObject obj)
    {
        if ((hideFlg_) ||                                   // 今隠れている
            (!obj.GetComponent<HideBox>().InFlagCheck()) || // 今入れない状態
            (Time.time - releaseTime_ <= coolTime_)||       // クールタイムが終わっていない
            (!Input.GetKey(KeyCode.E)))                     // Eキーが押されていない
        {
            return;
        }

        if ((lastInBoxObj_) && (lastInBoxObj_ != obj))
        {
            // 前回まで入っていた箱と違っている
            hideBox_.SetLastInFlag(false);
            hideBox_ = null;
        }
        lastInBoxObj_ = obj;

        // 入る箱の各オブジェクトを取得
        boxCamera_ = lastInBoxObj_.transform.Find("BoxCamera").gameObject;
        boxLamp_ = lastInBoxObj_.transform.Find("BoxLight").gameObject;
        hideBox_ = lastInBoxObj_.GetComponent<HideBox>();

        // 隠れる処理
        ChangeHideAction(true);
        hideBox_.SetLastInFlag(true);

        // 箱に入った回数加算
        hideNum++;
        Debug.Log("hideNum" + hideNum);

        hideFlg_ = true;
        stayTime_ = Time.time;
        releaseTime_ = 0.0f;
    }

    // 状態切り替え
    // true:箱に入る、false:箱から出る
    private void ChangeHideAction(bool flag)
    {
        if (IsBoxCamera())
        {
            // カメラの切り替え
            boxCamera_.SetActive(flag);
            mainCamera_.SetActive(!flag);
        }
        if (IsBoxLamp())
        {
            // 箱専用ライト
            boxLamp_.SetActive(flag);
        }
        if (IsHideBox())
        {
            // 箱に状態を送る
            hideBox_.SetInFlag(flag);
        }
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

    public bool GetHideFlg()
    {
        return hideFlg_;
    }
}
